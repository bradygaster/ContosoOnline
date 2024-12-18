﻿using ContosoOnline.OrderApi.Data;
using ContosoOnline.OrderApi.DataModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace ContosoOnline.OrderApi;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/orders").WithTags(nameof(Order));

        group.MapGet("/", async (OrderDbContext db) =>
        {
            return await db.Order.ToListAsync();
        })
        .WithName("GetAllOrders")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Order>, NotFound>> (Guid id, OrderDbContext db) =>
        {
            return await db.Order.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Order model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetOrderById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid id, Order order, OrderDbContext db) =>
        {
            var affected = await db.Order
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Processed, order.Processed)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateOrder")
        .WithOpenApi();

        group.MapPost("/", async (Order order, OrderDbContext db) =>
        {
            db.Order.Add(order);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/orders/{order.Id}",order);
        })
        .WithName("CreateOrder")
        .WithOpenApi();
    }
}
