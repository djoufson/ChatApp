namespace ChatApp.Mobile.Services.SignalR.Interfaces;

public interface IBaseConnection
{
    Task ConnectAsync();
    Task DisconnectAsync();
}
