namespace ChatApp.Api.Services;
public interface IFirebaseAdmin
{    
    Task PushAsync(string deviceToken, string title, string content, IDictionary<string, string> data);

    Task PushToChannelAsync(string channel, string title, string content);
}