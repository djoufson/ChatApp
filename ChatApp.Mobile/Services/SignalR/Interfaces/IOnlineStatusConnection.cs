namespace ChatApp.Mobile.Services.SignalR.Interfaces;

public interface IOnlineStatusConnection : IBaseConnection
{
    event EventHandler<OnlineStatusChangedEventArgs> OnlineStatusChanged;

    Task ChangeOnlineStatusAsync(bool status);
}
