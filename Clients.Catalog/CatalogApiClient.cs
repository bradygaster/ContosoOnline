using System.Text.Json;

namespace Clients;

public class CatalogApiClient(HttpClient httpClient)
{
    JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public async Task<IEnumerable<CatalogItem>?> GetCatalogItemsAsync()
    {
        var response = await httpClient.GetAsync("/products");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<CatalogItem>>(content, options);
    }
    public async Task<CatalogItem?> GetCatalogItemByIdAsync(int id)
    {
        var response = await httpClient.GetAsync($"/products/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CatalogItem>(content, options);
    }
}

public class CatalogItem
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
}
