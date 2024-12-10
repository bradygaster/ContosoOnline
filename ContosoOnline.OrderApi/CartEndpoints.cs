using Microsoft.EntityFrameworkCore;
using ContosoOnline.OrderApi.Data;
using ContosoOnline.OrderApi.DataModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ContosoOnline.OrderApi;

public static class CartEndpoints
{
    public static void MapCartEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Cart").WithTags(nameof(Cart));

        group.MapGet("/", async (OrderDbContext db) =>
        {
            return await db.Cart.ToListAsync();
        })
        .WithName("GetAllCarts")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Cart>, NotFound>> (Guid id, OrderDbContext db) =>
        {
            return await db.Cart.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Cart model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCartById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid id, Cart cart, OrderDbContext db) =>
        {
            var affected = await db.Cart
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, cart.Id)
                    .SetProperty(m => m.Started, cart.Started)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCart")
        .WithOpenApi();

        group.MapPost("/", async (Cart cart, OrderDbContext db) =>
        {
            db.Cart.Add(cart);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Cart/{cart.Id}",cart);
        })
        .WithName("CreateCart")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid id, OrderDbContext db) =>
        {
            var affected = await db.Cart
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCart")
        .WithOpenApi();
    }
}
