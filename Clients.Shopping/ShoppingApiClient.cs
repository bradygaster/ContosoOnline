using System.Net.Http.Json;

namespace Clients;

public class ShoppingApiClient(HttpClient httpClient)
{
    public async Task CreateCartAsync(Cart cart)
    {
        await httpClient.PostAsJsonAsync<Cart>($"/carts", cart);
    }

    public async Task<Cart?> GetCartAsync(Guid cartId)
    {
        var cart = await httpClient.GetFromJsonAsync<Cart>($"/carts/{cartId}");
        cart!.Items = (await httpClient.GetFromJsonAsync<List<CartItem>>($"/carts/{cartId}/items")) ?? new List<CartItem>();
        return cart;
    }

    public async Task AddItemToCart(CartItem item)
        => await httpClient.PostAsJsonAsync<CartItem>($"/carts/{item.CartId}/items", item);

    public async Task UpdateItemInCart(CartItem item)
        => await httpClient.PutAsJsonAsync<CartItem>($"/carts/{item.CartId}/items/{item.Id}", item);

    public async Task DeleteCartItem(Guid itemId, Guid cartId)
        => await httpClient.DeleteAsync($"/carts/{cartId}/items/{itemId}");
}

public class Cart
{
    public Guid Id { get; set; }
    public DateTime Started { get; set; }
    public List<CartItem> Items { get; set; } = new();
}

public class Order
{
    public Guid Id { get; set; }
    // Other properties
}

public class CartItem
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}