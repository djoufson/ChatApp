namespace ChatApp.Mobile.Services.SignalR.Interfaces
{
    public interface IMessageConnection
    {
        event EventHandler<string> OnMessageRecieved;
        Task ConnectAsync();
        Task DisconnectAsync();
        Task SendMessageAsync(string text);
    }
}
