using ChatApp.Shared.Utilities.EventArgs;
using Microsoft.AspNetCore.SignalR.Client;
using static System.Net.Mime.MediaTypeNames;

namespace ChatApp.Mobile.Services.SignalR.Concrete;

internal class MessageConnection : IMessageConnection
{
    public event EventHandler<RecievedMessageEventArg> OnMessageRecieved;
    private HubConnection _connection;
    public MessageConnection()
    {
        _connection = new HubConnectionBuilder()
        #if ANDROID && DEBUG
            .WithUrl("https://192.168.8.100:7177/messages")
        #else
			.WithUrl("https://localhost:7177/messages")
        #endif
            .Build();
        
        Task.Run(async () => await ConnectAsync());
        _connection.On<RecievedMessageEventArg>(EventNames.MessageRecieved, MessageRecieved);
    }
    private void MessageRecieved(RecievedMessageEventArg message)
    {
        OnMessageRecieved?.Invoke(this, message);
    }
    public Task ConnectAsync()
    {
        return _connection.StartAsync();
    }

    public Task DisconnectAsync()
    {
        return _connection.StopAsync();
    }

    public Task SendMessageToAsync(string toUserEmail, string content)
    {
        return _connection.InvokeCoreAsync(EventNames.SendMessageToUser, new[] { toUserEmail, content });
    }
}
