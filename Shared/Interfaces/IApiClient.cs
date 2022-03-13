namespace Shared.Interfaces
{
    public interface IApiClient
    {
        Task<HttpClient> GetClientAsync();
    }
}