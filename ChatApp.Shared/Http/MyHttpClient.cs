using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using static ChatApp.Shared.Utilities.Enums;

namespace ChatApp.Shared.Http;

public class MyHttpClient : IMyHttpClient
{
    private readonly HttpClient _client;

    // CONSTRUCTOR
    public MyHttpClient()
    {
        _client = new HttpClient();
    }

    // CONSTRUCTOR
    public MyHttpClient(string baseUrl)
    {
        _client = new HttpClient()
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    // CONSTRUCTOR
    public MyHttpClient(string baseUrl, double timeout)
    {
        _client = new HttpClient()
        {
            BaseAddress = new Uri(baseUrl),
            Timeout = TimeSpan.FromSeconds(timeout)
        };
    }

    public MyHttpClient UseBaseUrl(string baseUrl)
    {
        _client.BaseAddress = new Uri(baseUrl);
        return this;
    }

    public MyHttpClient UseTimeOut(double timeout)
    {
        _client.Timeout = TimeSpan.FromSeconds(timeout);
        return this;
    }

    public void Dispose() => _client.Dispose();

    public Task<T> PostAsync<T>(string url, Dictionary<string, string> content, string? auth = null)
        => SendAsync<T>(MyHttpMethods.POST, url, content, auth);

    public Task<T> GetAsync<T>(string url, string? auth = null)
        => SendAsync<T>(MyHttpMethods.GET, url, null, auth);

    public async Task<T> SendAsync<T>(MyHttpMethods method, string url, Dictionary<string, string>? content = null, string? auth = null)
    {
        JsonContent? stringContent = null;
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (!string.IsNullOrEmpty(auth))
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);

        if (content != null)
            stringContent = JsonContent.Create(content);

        HttpResponseMessage httpResponseMessage = null!;
        switch (method)
        {
            case MyHttpMethods.GET:
                {
                    var sb = new StringBuilder();
                    if (content is not null)
                    {
                        sb.Append('?');
                        foreach (var item in content)
                        {
                            sb.Append(item.Key + "=" + item.Value);
                        }

                        url += sb.ToString();
                    }

                    httpResponseMessage = await _client.GetAsync(url);
                }
                break;
            case MyHttpMethods.PUT:
                httpResponseMessage = await _client.PutAsync(url, stringContent);
                break;
            case MyHttpMethods.POST:
                httpResponseMessage = await _client.PostAsync(url, stringContent);
                break;
            case MyHttpMethods.DELETE:
                break;
            case MyHttpMethods.HEAD:
                break;
            case MyHttpMethods.OPTIONS:
                break;
            case MyHttpMethods.TRACE:
                break;
            case MyHttpMethods.PATCH:
                break;
            case MyHttpMethods.CONNECT:
                break;
            default:
                break;
        }

        if (httpResponseMessage is null) return default!;
        var resultStr = await httpResponseMessage.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert.DeserializeObject<T>(resultStr)!;

        return apiResponse;
    }
    
}