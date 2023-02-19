namespace ChatApp.Mobile.Services.SignalR.Interfaces;

public interface IMessageConnection : IBaseConnection
{
    event EventHandler<RecievedMessageEventArg> OnMessageRecieved;
    Task SendMessageToAsync(string toUserEmail, MessageWithoutEntities content);
}
