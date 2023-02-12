namespace ChatApp.Mobile.Services.SignalR.Interfaces;

internal interface IMessageStatusConnection : IBaseConnection
{
    event EventHandler<MessageDeliveredEventArgs> OnMessageDelivered;
    event EventHandler<ConversationOpenedEventArgs> OnConversationOpened;

    Task DeliverMessageAsync(string issuerMail, int messageId, int conversationId);
    Task OpenConversationAsync(int conversationId, string toUserEmail);
}
