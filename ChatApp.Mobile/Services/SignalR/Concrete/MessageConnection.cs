using Microsoft.AspNetCore.SignalR.Client;

namespace ChatApp.Mobile.Services.SignalR.Concrete;

internal class MessageConnection : IMessageConnection
{
    public event EventHandler<string> OnMessageRecieved;
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
        _connection.On<string>(EventNames.MessageRecieved, MessageRecieved);
    }
    private void MessageRecieved(string message)
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

    public Task SendMessageAsync(string text)
    {
        return _connection.InvokeCoreAsync(EventNames.SendMessage, new[] { text });
    }
}
