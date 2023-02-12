namespace ChatApp.Mobile.Services.SignalR.Concrete;

public class MessageConnection : IMessageConnection
{
    public event EventHandler<RecievedMessageEventArg> OnMessageRecieved;
    private readonly HubConnection _connection;
    public MessageConnection()
    {
        _connection = new HubConnectionBuilder()
			.WithUrl($"https://localhost:7177/{HubRoutes.MessagesRoute}")
            .Build();
        
        Task.Run(async () => await ConnectAsync());
        _connection.On<RecievedMessageEventArg>(EventNames.MessageRecieved, MessageRecieved);
    }

    #region EVENT HANDLERS
    private void MessageRecieved(RecievedMessageEventArg message)
    {
        OnMessageRecieved?.Invoke(this, message);
    }
    #endregion

    #region PUBLIC INTERFACE
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
    #endregion
}
