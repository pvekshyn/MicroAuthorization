using Roles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPermissionRepository, PermissionRepository>()
    .AddSingleton<IRoleRepository, RoleRepository>();
builder.Services.Configure<RolesSettings>(builder.Configuration);

builder.Services.AddCap(x =>
{
    x.UseSqlServer(opt =>
    {
        opt.ConnectionString = builder.Configuration.GetConnectionString("Database");
    });
    x.UseRabbitMQ("localhost");
});

var app = builder.Build();

app.MapGet("/permission/{id}", async (Guid id, IPermissionRepository repository) =>
{
    var permission = await repository.GetAsync(id);
    return permission is null
        ? Results.NotFound()
        : Results.Ok(permission);
});

app.MapPost("/permission", async (Permission permission, IPermissionRepository repository) =>
{
    await repository.AddAsync(permission);
});

app.MapDelete("/permission/{id}", async (Guid id, IPermissionRepository repository) =>
{
    await repository.RemoveAsync(id);
});

app.MapGet("/role/{id}", async (Guid id, IRoleRepository repository) =>
{
    var role = await repository.GetAsync(id);
    return role is null
        ? Results.NotFound()
        : Results.Ok(role);
});

app.MapPost("/role", async (Role role, IRoleRepository repository) =>
{
    await repository.AddAsync(role);
});

app.MapPut("/role/{id}/add/permission/{permissionId}", async (Guid id, Guid permissionId, IRoleRepository repository) =>
{
    await repository.AddPermissionToRoleAsync(id, permissionId);
});

app.MapPut("/role/{id}/remove/permission/{permissionId}", async (Guid id, Guid permissionId, IRoleRepository repository) =>
{
    await repository.RemovePermissionFromRoleAsync(id, permissionId);
});


app.MapDelete("/role/{id}", async (Guid id, IRoleRepository repository) =>
{
    await repository.RemoveAsync(id);
});

app.Run();

public partial class Program { }
