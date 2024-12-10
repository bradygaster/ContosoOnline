using ContosoOnline.OrderApi.Data;
using ContosoOnline.OrderApi.DataModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace ContosoOnline.OrderApi;

public static class CartItemEndpoints
{
    public static void MapCartItemEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/items").WithTags(nameof(CartItem));

        group.MapGet("/", async (OrderDbContext db) =>
        {
            return await db.CartItem.ToListAsync();
        })
        .WithName("GetAllCartItems")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<CartItem>, NotFound>> (Guid id, OrderDbContext db) =>
        {
            return await db.CartItem.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is CartItem model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCartItemById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid id, CartItem cartItem, OrderDbContext db) =>
        {
            var affected = await db.CartItem
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, cartItem.Id)
                    .SetProperty(m => m.CartId, cartItem.CartId)
                    .SetProperty(m => m.ProductId, cartItem.ProductId)
                    .SetProperty(m => m.Quantity, cartItem.Quantity)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCartItem")
        .WithOpenApi();

        group.MapPost("/", async (CartItem cartItem, OrderDbContext db) =>
        {
            db.CartItem.Add(cartItem);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/items/{cartItem.Id}",cartItem);
        })
        .WithName("CreateCartItem")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid id, OrderDbContext db) =>
        {
            var affected = await db.CartItem
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCartItem")
        .WithOpenApi();
    }
}
