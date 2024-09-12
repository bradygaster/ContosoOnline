using ContosoOnline.Workers.OrderProcessor;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
