using OtherApp.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;

namespace OtherApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IApiClient _apiClient;

        public ProductsController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = await _apiClient.GetClientAsync();
            return View(await httpClient.GetProductsAsync());
        }
    }
}