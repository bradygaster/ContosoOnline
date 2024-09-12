using Microsoft.EntityFrameworkCore;
using ContosoOnline.OrderApi.Data;
using ContosoOnline.OrderApi.DataModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ContosoOnline.OrderApi;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Order").WithTags(nameof(Order));

        group.MapGet("/", async (ContosoOnlineOrderApiDbContext db) =>
        {
            return await db.Order.ToListAsync();
        })
        .WithName("GetAllOrders")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Order>, NotFound>> (int id, ContosoOnlineOrderApiDbContext db) =>
        {
            return await db.Order.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Order model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetOrderById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Order order, ContosoOnlineOrderApiDbContext db) =>
        {
            var affected = await db.Order
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, order.Id)
                    .SetProperty(m => m.CartId, order.CartId)
                    .SetProperty(m => m.Received, order.Received)
                    .SetProperty(m => m.Processed, order.Processed)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateOrder")
        .WithOpenApi();

        group.MapPost("/", async (Order order, ContosoOnlineOrderApiDbContext db) =>
        {
            db.Order.Add(order);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Order/{order.Id}",order);
        })
        .WithName("CreateOrder")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ContosoOnlineOrderApiDbContext db) =>
        {
            var affected = await db.Order
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteOrder")
        .WithOpenApi();
    }
}
