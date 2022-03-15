using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Shared.Interfaces;
using Shared.Models;
using Microsoft.AspNetCore.Authentication;

namespace OtherApp.Services
{
    public class ApiClient : IApiClient
    {
        private readonly Client _client;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClient(IHttpContextAccessor httpContextAccessor, IOptions<Client> clientOptions)
        {
            _client = clientOptions.Value;
            _httpClient = new HttpClient();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpClient> GetClientAsync()
        {
            var accessToken = await _httpContextAccessor.HttpContext!.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            _httpClient.SetBearerToken(accessToken);

            return _httpClient;
        }
    }
}