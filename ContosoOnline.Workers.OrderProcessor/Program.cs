using ContosoOnline.Workers.OrderProcessor;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpClient<OrdersApiClient>(client =>
{
    client.BaseAddress = new("https://localhost:7144");
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
