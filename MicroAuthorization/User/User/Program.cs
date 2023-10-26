using System.ComponentModel.DataAnnotations;
using Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.Configure<UserSettings>(builder.Configuration);

builder.Services.AddCap(x =>
{
    x.UseMongoDB(opt =>
    {
        opt.DatabaseConnection = builder.Configuration.GetConnectionString("Database");
    });
    x.UseRabbitMQ("localhost");
});

builder.Services.AddExceptionHandler<ExceptionHandler>();

var app = builder.Build();

app.MapGet("/user/{id}", async (Guid id, IUserRepository repository) =>
{
    var user = await repository.GetAsync(id);
    return user is null
        ? Results.NotFound()
        : Results.Ok(permission);
});

app.MapPost("/user", async (User user, IUserRepository repository) =>
{
    if (!user.Email.Contains('.') || !user.Email.Contains('@'))
        throw new ValidationException("Invalid Email");

    await repository.AddAsync(user);
});

app.MapDelete("/user/{id}", async (Guid id, IUserRepository repository) =>
{
    await repository.RemoveAsync(id);
});

app.UseExceptionHandler(opt => { });

app.Run();
