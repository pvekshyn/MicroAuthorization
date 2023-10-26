using ApiGateway;
using Refit;

var builder = WebApplication.CreateBuilder(args);

var configFilename = builder.Configuration["GatewayConfig"];
var identityServerUrl = builder.Configuration.GetServiceUri("identity-server")?.ToString();
var accessUrl = builder.Configuration.GetServiceUri("access-api")?.ToString();

var apiGatewayConfig = new ConfigurationBuilder()
    .AddJsonFile(configFilename)
    .Build();
builder.Services.AddReverseProxy()
    .LoadFromConfig(apiGatewayConfig.GetSection("ReverseProxy"));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = identityServerUrl;
        options.TokenValidationParameters.ValidateAudience = false;
        options.RequireHttpsMetadata = false;
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

var settings = new RefitSettings
{
    ExceptionFactory = httpResponse => Task.FromResult<Exception>(null)
};
var accessApiClient = RestService.For<IAccessApi>(accessUrl, settings);

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use(async (context, next) =>
    {
        var proxyFeature = context.GetReverseProxyFeature();

        if (proxyFeature.Route.Config.Metadata?.TryGetValue("PermissionId", out var permissionIdString) ?? false)
        {
            var permissionId = Guid.Parse(permissionIdString);

            var claim = context.User.Claims.First(c => c.Type == "client_sub");
            var userId = Guid.Parse(claim.Value);

            var result = await accessApiClient.CheckAccessAsync(userId, permissionId);
            if (!result.IsSuccessStatusCode)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }
        }

        await next();
    });
});

app.Run();
