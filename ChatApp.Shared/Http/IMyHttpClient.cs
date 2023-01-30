using ChatApp.Shared.Utilities;

namespace ChatApp.Shared.Http
{
    public interface IMyHttpClient : IDisposable
    {
        Task<T> GetAsync<T>(string url, string? auth = null);
        Task<T> PostAsync<T>(string url, Dictionary<string, string> content, string? auth = null);
        Task<T> SendAsync<T>(Enums.MyHttpMethods method, string url, Dictionary<string, string>? content = null, string? auth = null);
        MyHttpClient UseBaseUrl(string baseUrl);
        MyHttpClient UseTimeOut(double timeout);
    }
}