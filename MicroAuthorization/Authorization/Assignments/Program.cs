using Assignments;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAssignmentRepository, AssignmentRepository>();
builder.Services.Configure<AssignmentSettings>(builder.Configuration);

builder.Services.AddCap(x =>
{
    x.UseSqlServer(opt =>
    {
        opt.ConnectionString = builder.Configuration.GetConnectionString("Database");
    });
    x.UseRabbitMQ("localhost");
});

var app = builder.Build();

app.MapGet("/assignment/role/{roleId}/user/{userId}", async (Guid roleId, Guid userId, IAssignmentRepository repository) =>
{
    var assignment = await repository.GetAsync(roleId, userId);
    return assignment is null
        ? Results.NotFound()
        : Results.Ok(assignment);
});

app.MapPost("/assign", async (Assignment assignment, IAssignmentRepository repository) =>
{
    await repository.AddAsync(assignment);
});

app.MapPost("/deassign/role/{roleId}/user/{userId}", async (Guid roleId, Guid userId, IAssignmentRepository repository) =>
{
    await repository.RemoveAsync(roleId, userId);
});

app.Run();

public partial class Program { }
