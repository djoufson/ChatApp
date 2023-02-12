namespace ChatApp.Mobile.Services.SignalR.Concrete;

public class MessageStatusConnection : IMessageStatusConnection
{
    private readonly HubConnection _connection;

    public event EventHandler<MessageDeliveredEventArgs> OnMessageDelivered;
    public event EventHandler<ConversationOpenedEventArgs> OnConversationOpened;

    // CONSTRUCTOR
    public MessageStatusConnection()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"https://localhost:7177/{HubRoutes.MessagesRoute}")
            .Build();

        Task.Run(async () => await ConnectAsync());
        _connection.On<MessageDeliveredEventArgs>(EventNames.MessageDelivered, MessageDelivered);
        _connection.On<ConversationOpenedEventArgs>(EventNames.ConversationOpened, ConversationOpened);
    }

    #region EVENT HANDLERS
    public void MessageDelivered(MessageDeliveredEventArgs e)
    {
        OnMessageDelivered?.Invoke(this, e);
    }
    public void ConversationOpened(ConversationOpenedEventArgs e)
    {
        OnConversationOpened?.Invoke(this, e);
    }
    #endregion

    #region PUBLIC INTERFACE
    public Task ConnectAsync()
    {
        return _connection.StartAsync();
    }

    public Task DeliverMessageAsync(string issuerMail, int messageId, int conversationId)
    {
        return _connection.InvokeAsync(EventNames.DeliverMessage, issuerMail, messageId, conversationId);
    }

    public Task DisconnectAsync()
    {
        return _connection.StopAsync();
    }
    public Task OpenConversationAsync(int conversationId, string toUserEmail)
    {
        return _connection.InvokeAsync(EventNames.OpenConversation, conversationId, toUserEmail);
    }
    #endregion
}
