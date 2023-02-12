using ChatApp.Shared.Utilities.EventArgs;

namespace ChatApp.Mobile.Services.SignalR.Interfaces
{
    public interface IMessageConnection
    {
        event EventHandler<RecievedMessageEventArg> OnMessageRecieved;
        Task ConnectAsync();
        Task DisconnectAsync();
        Task SendMessageToAsync(string toUserEmail, string content);
    }
}
