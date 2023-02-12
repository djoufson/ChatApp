namespace ChatApp.Mobile.Services.SignalR.Interfaces;

public interface IGroupConnection : IBaseConnection
{
    event EventHandler<GroupJoinedEventArg> OnGroupJoined;
    event EventHandler<AddedToGroupEventArgs> OnAddedToGroupEventArgs;
    event EventHandler<RemovedFromGroupEventArgs> OnRemovedFromGroup;
    event EventHandler<GroupLeftEventArgs> OnGroupLeft;

    Task RestoreGroupConnectionAsync(int groupId);
    Task JoinGroupAsync(int groupId);
    Task RemoveFromGroupAsync(int groupId, string email);
    Task LeaveGroupAsync(int groupId);
    Task AddToGroupAsync(int groupId, string email);
}
