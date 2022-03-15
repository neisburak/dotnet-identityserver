using System.Text.Json;
using Shared.Models;

namespace ClientApp.Services
{
    public static class ProductManager
    {
        public static async Task<List<Product>> GetProductsAsync(this HttpClient httpClient)
        {
            var response = await httpClient.GetAsync("https://localhost:7001/api/items");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<List<Product>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return products ?? new();
            }
            return new();
        }
    }
}