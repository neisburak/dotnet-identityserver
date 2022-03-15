using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Shared.Interfaces;
using Shared.Models;

namespace ClientApp.Services
{
    public class ApiClient : IApiClient
    {
        private readonly Client _client;
        private readonly HttpClient _httpClient;
        private DateTime _expiration;

        public ApiClient(IOptions<Client> clientOptions)
        {
            _client = clientOptions.Value;
            _expiration = DateTime.Now;
            _httpClient = new HttpClient();
        }

        public async Task<HttpClient> GetClientAsync()
        {
            if (DateTime.Now > _expiration)
            {
                var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync("https://localhost:7000");
                if (discoveryDocument.IsError)
                {
                    // Handle error
                }

                var tokenRequest = new ClientCredentialsTokenRequest
                {
                    ClientId = _client.Id,
                    ClientSecret = _client.Secret,
                    Address = discoveryDocument.TokenEndpoint
                };
                var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(tokenRequest);
                if (tokenResponse.IsError)
                {
                    // Handle error
                }
                _httpClient.SetBearerToken(tokenResponse.AccessToken);
                _expiration = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);
            }

            return _httpClient;
        }
    }
}