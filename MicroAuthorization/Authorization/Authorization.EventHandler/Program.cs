using Authorization.EventHandler;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.Configure<AuthorizationSettings>(builder.Configuration);

builder.Services.AddCap(x =>
{
    x.UseSqlServer(opt =>
    {
        opt.ConnectionString = builder.Configuration.GetConnectionString("Database");
    });
    x.UseRabbitMQ("localhost");
});

builder.Services.AddSingleton<EventSubscriber>();

var host = builder.Build();
host.Run();
