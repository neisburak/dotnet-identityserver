using App.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;

namespace App.Controllers
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