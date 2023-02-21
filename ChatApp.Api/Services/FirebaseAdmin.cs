using System.Diagnostics;
using Newtonsoft.Json;

namespace ChatApp.Api.Services;

public class FirebaseAdmin : IFirebaseAdmin
{
    public async Task PushAsync(string? deviceToken, string title, string content, IDictionary<string, string>? data)
    {
        if(deviceToken is null) return;
        var url = "https://fcm.googleapis.com/fcm/send";
        var notificationModel = new 
        {
            notification = new 
            {
                Title = title,
                Body = content
            },
            registration_ids = new List<string>{ deviceToken }
        };

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", "="+"AAAAY0a9u2Y:APA91bFwzRCuE0rqxPdhU_OkbfTB4SvNBY5vQxOrdhNkT3dXXuifbem7HuydLO0fjR_UCg-dklUxvbcr6AjM6LsAfD0qvgVWXPB1wlCcGwGogzUYUqXKf-NqKUzApq8UqVz4IyqwqbJR");
        var serializedContent = JsonConvert.SerializeObject(notificationModel);
        var response = await client.PostAsync(url, new StringContent(serializedContent, Encoding.UTF8, "application/json"));
        if(response.IsSuccessStatusCode)
            Debug.WriteLine("Notification Sent");
    }
    
    public Task PushToChannelAsync(string channel, string title, string content)
    {
        throw new NotImplementedException();
    }
}