namespace ChatApp.Mobile.Services.SignalR.Concrete;

public class MessageConnection : IMessageConnection
{
    public event EventHandler<RecievedMessageEventArg> OnMessageRecieved;
    private readonly HubConnection _connection;
    public MessageConnection()
    {
        _connection = new HubConnectionBuilder()
			.WithUrl($"https://localhost:7177/{HubRoutes.MessagesRoute}", (options) =>
            {
                options.Headers.Add("access_token", App.AuthToken);
            })
            .Build();
        
        Task.Run(() =>
        {
            App.Current.MainPage.Dispatcher.Dispatch(async () =>
            {
                try
                {
                    await ConnectAsync();
                }
                catch (Exception) { }
            });
        });
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

    public Task SendMessageToAsync(string toUserEmail, MessageWithoutEntities content)
    {
        return _connection.InvokeCoreAsync(EventNames.SendMessageToUser, new object[] { toUserEmail, content });
    }
    #endregion
}
