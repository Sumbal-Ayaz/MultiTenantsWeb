namespace MultiTenants.Web.Interfaces
{
    public interface IApiClient
    {
        Task<T?> SendAsync<T>(HttpRequestMessage request);
        Task<TResponse> GetAsync<TResponse>(string url);
        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data);
        Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest data);
        Task DeleteAsync(string url);
    }
}
