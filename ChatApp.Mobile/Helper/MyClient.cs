using ChatApp.Mobile.Utilities;

namespace ChatApp.Mobile.Helper;

public class MyClient
{
    public static async Task<T> SendRequestAsync<T>(MyHttpMethods method, string url, Dictionary<string, string> content = null, string auth = null)
    {
#if DEBUG
        var devSslHelper = new DevHttpsConnectionHelper(7177);
        using var client = devSslHelper.HttpClient;
#else
		using var client = new HttpClient();
#endif
        client.BaseAddress = new Uri($"{devSslHelper.DevServerRootUrl}/{Constants.BaseUrl}");
        JsonContent stringContent = null;
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (!string.IsNullOrEmpty(auth))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);

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

                    httpResponseMessage = await client.GetAsync(url);
                }
                break;
            case MyHttpMethods.PUT:
                httpResponseMessage = await client.PutAsync(url, stringContent);
                break;
            case MyHttpMethods.POST:
                httpResponseMessage = await client.PostAsync(url, stringContent);
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

        if (httpResponseMessage is null) return default;
        var resultStr = await httpResponseMessage.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert.DeserializeObject<T>(resultStr)!;

        return apiResponse;
    }
}
