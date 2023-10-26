using Access;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAccessRepository, AccessRepository>();
builder.Services.Configure<AccessSettings>(builder.Configuration);

var app = builder.Build();

app.MapGet("/check-access/user/{userId}/permission/{permissionId}", async (Guid userId, Guid permissionId, IAccessRepository repository) =>
{
    var hasAccess = await repository.CheckAccessAsync(userId, permissionId);
    return hasAccess
        ? Results.Ok()
        : Results.StatusCode((int)HttpStatusCode.Forbidden);
});

app.Run();

public partial class Program { }
