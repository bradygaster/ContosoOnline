using System.Net.Http.Json;

namespace ContosoOnline.Workers.OrderProcessor;

internal class Worker(ILogger<Worker> logger, OrdersApiClient ordersApiClient) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var orders = await ordersApiClient.GetOrders();
            logger.LogInformation($"There are {orders!.Count(x => x.Processed == null)} orders to process.");

            foreach (var order in orders!.Where(x => x.Processed == null))
            {
                logger.LogInformation($"Processing order {order.Id}.");
                order.Processed = DateTime.UtcNow;
                await ordersApiClient.ProcessOrder(order);
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}

internal class OrdersApiClient(HttpClient httpClient)
{
    internal async Task<List<Order>?> GetOrders()
        => await httpClient.GetFromJsonAsync<List<Order>>("/orders");
    internal async Task ProcessOrder(Order order)
        => await httpClient.PutAsJsonAsync($"/orders/{order.Id}", order);
}

public class Order
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public DateTime? Processed { get; set; }
}