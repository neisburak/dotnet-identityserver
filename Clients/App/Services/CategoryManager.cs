using System.Text.Json;
using Shared.Models;

namespace App.Services
{
    public static class CategoryManager
    {
        public static async Task<List<Category>> GetCategoriesAsync(this HttpClient httpClient)
        {
            var response = await httpClient.GetAsync("https://localhost:7002/api/items");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var categories = JsonSerializer.Deserialize<List<Category>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return categories ?? new();
            }
            return new();
        }
    }
}