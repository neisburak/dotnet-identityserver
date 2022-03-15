using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using PwdApp.Models;
using Shared.Models;

namespace PwdApp.Controllers
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

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var httpClient = new HttpClient();
                var discoveryDocument = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7000");
                if (discoveryDocument.IsError)
                {
                    // Handle error
                }

                var request = new PasswordTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    UserName = viewModel.UserName,
                    Password = viewModel.Password,
                    ClientId = _client.Id,
                    ClientSecret = _client.Secret
                };
                var response = await httpClient.RequestPasswordTokenAsync(request);
                if (response.IsError)
                {
                    // Handle error
                }

                var userInfoRequest = new UserInfoRequest
                {
                    Token = response.AccessToken,
                    Address = discoveryDocument.UserInfoEndpoint,
                };
                var userInfoResponse = await httpClient.GetUserInfoAsync(userInfoRequest);
                if (userInfoResponse.IsError)
                {
                    // Handle error
                }

                var identity = new ClaimsIdentity(userInfoResponse.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
                var principle = new ClaimsPrincipal(identity);

                var authenticationProperties = new AuthenticationProperties();
                authenticationProperties.StoreTokens(new List<AuthenticationToken>
                {
                    new() { Name = OpenIdConnectParameterNames.AccessToken, Value = response.AccessToken },
                    new() { Name = OpenIdConnectParameterNames.RefreshToken, Value = response.RefreshToken },
                    new() { Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.UtcNow.AddSeconds(response.ExpiresIn).ToString("o", CultureInfo.InvariantCulture) },
                });

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle, authenticationProperties);

                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var httpClient = new HttpClient();
                var discoveryDocument = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7000");
                if (discoveryDocument.IsError)
                {
                    // Handle error
                }

                var request = new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = _client.Id,
                    ClientSecret = _client.Secret
                };
                var response = await httpClient.RequestClientCredentialsTokenAsync(request);
                if (response.IsError)
                {
                    // Handle error
                }

                httpClient.SetBearerToken(response.AccessToken);

                var requestString = new StringContent(JsonSerializer.Serialize(viewModel), Encoding.UTF8, "application/json");
                var registerResult = await httpClient.PostAsync("https://localhost:7000/account/register", requestString);
                var registerResponse = await registerResult.Content.ReadAsStringAsync();
                if (registerResult.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, registerResponse);
                }
            }
            return View(viewModel);
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