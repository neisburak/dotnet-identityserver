using System.Globalization;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Shared.Models;

namespace App.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly Client _client;

        public UserController(IOptions<Client> client)
        {
            _client = client.Value;
        }

        public IActionResult Index() => View();

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> RefreshToken()
        {
            var httpClient = new HttpClient();
            var discoveryDocument = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7000");
            if (discoveryDocument.IsError)
            {
                // Handle error
            }

            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            var request = new RefreshTokenRequest
            {
                ClientId = _client.Id,
                ClientSecret = _client.Secret,
                RefreshToken = refreshToken,
                Address = discoveryDocument.TokenEndpoint,
            };
            var response = await httpClient.RequestRefreshTokenAsync(request);
            if (response.IsError)
            {
                // Handle error
            }

            var authenticationTokens = new List<AuthenticationToken>
            {
                new() { Name = OpenIdConnectParameterNames.IdToken, Value = response.IdentityToken },
                new() { Name = OpenIdConnectParameterNames.AccessToken, Value = response.AccessToken },
                new() { Name = OpenIdConnectParameterNames.RefreshToken, Value = response.RefreshToken },
                new() { Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.UtcNow.AddSeconds(response.ExpiresIn).ToString("o", CultureInfo.InvariantCulture) }
            };

            var authenticationResult = await HttpContext.AuthenticateAsync();
            var properties = authenticationResult.Properties;

            properties?.StoreTokens(authenticationTokens);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal!, properties);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public IActionResult Dasboard() => View();

        [Authorize(Roles = "admin, customer")]
        public IActionResult Customers() => View();
    }
}