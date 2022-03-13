using Microsoft.AspNetCore.Mvc;
using OtherApp.Services;
using Shared.Interfaces;

namespace OtherApp.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IApiClient _apiClient;
        public CategoriesController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = await _apiClient.GetClientAsync();
            return View(await httpClient.GetCategoriesAsync());
        }
    }
}