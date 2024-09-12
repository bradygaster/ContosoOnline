using Microsoft.EntityFrameworkCore;
using ContosoOnline.OrderApi.Data;
using ContosoOnline.OrderApi.DataModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ContosoOnline.OrderApi;

public static class CartItemEndpoints
{
    public static void MapCartItemEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/CartItem").WithTags(nameof(CartItem));

        group.MapGet("/", async (ContosoOnlineOrderApiDbContext db) =>
        {
            return await db.CartItem.ToListAsync();
        })
        .WithName("GetAllCartItems")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<CartItem>, NotFound>> (int id, ContosoOnlineOrderApiDbContext db) =>
        {
            return await db.CartItem.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is CartItem model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCartItemById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, CartItem cartItem, ContosoOnlineOrderApiDbContext db) =>
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

        group.MapPost("/", async (CartItem cartItem, ContosoOnlineOrderApiDbContext db) =>
        {
            db.CartItem.Add(cartItem);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/CartItem/{cartItem.Id}",cartItem);
        })
        .WithName("CreateCartItem")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ContosoOnlineOrderApiDbContext db) =>
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
