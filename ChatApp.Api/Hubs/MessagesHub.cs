using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Api.Hubs;

public class MessagesHub : Hub
{
    public async Task SendMessage(string message)
    {
        Console.WriteLine(message);
        await Clients.All.SendAsync("MessageRecieved", message);
    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}
