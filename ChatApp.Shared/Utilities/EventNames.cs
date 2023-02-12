namespace ChatApp.Shared.Utilities;

public class EventNames
{
    // Messages Events
    public const string SendMessageToUser = "SendMessageToUser";
    public const string SendMessageToGroup = "SendMessageToGroup";
    public const string MessageRecieved = "MessageRecieved";
    public const string MessageRecievedFromGroup = "MessageRecievedFromGroup";

    // Connection status events (Online / Offline)
    public const string StatusOnlineChanged = "StatusOnlineChanged";
    public const string ChangeOnlineStatus = "ChangeOnlineStatus";

    // Writing status events (Writing if someone is writing)
    public const string WritingStatusChanged = "WritingStatusChanged";
    public const string ChangeWritingStatus = "ChangeWritingStatus";

    // Message status events (Sent, Delivered, Opened)
    //public const string MessageSent = "MessageSent"; // Removed because the sent status is not shared across the network
    public const string MessageDelivered = "MessageDelivered"; // The delivered status istead is shared to other users
    public const string MessageOpened = "MessageOpened"; // As well as the opend status
    public const string DeliverMessage = "DeliverMessage";
    public const string OpenMessage = "OpenMessage";

    // Group status events
    public const string RestoreGroup = "RestoreGroup";
    public const string GroupJoined = "GroupJoined";
    public const string GroupLeft = "GroupLeft";
    public const string AddedToGroup = "AddedToGroup";
    public const string RemovedFromGroup = "RemovedFromGroup";
    public const string JoinGroup = "JoinGroup";
    public const string AddToGroup = "AddToGroup";
    public const string RemoveFromGroup = "RemoveFromGroup";
    public const string LeaveGroup = "LeaveGroup";
}
