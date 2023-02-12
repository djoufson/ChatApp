using ChatApp.Shared.Dtos;

namespace ChatApp.Shared.Utilities.EventArgs;

public class RecievedMessageEventArg
{
    public bool Status { get; set; }
    public string IssuerName { get; set; } = null!;
    public string IssuerEmail { get; set; } = null!;
    public MessageWithoutEntities Message { get; set; } = null!;
}

public class GroupJoinedEventArg
{
    public bool Status { get; set; }
    public int GroupId { get; set; }
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
}

public class AddedToGroupEventArgs
{
    public bool Status { get; set; }
    public int GroupId { get; set; }
    public string IssuerName { get; set; } = null!;
    public string IssuerEmail { get; set; } = null!;
    public string NewUserName { get; set; } = null!;
    public string NewUserEmail { get; set; } = null!;
}

public class RemovedFromGroupEventArgs
{
    public bool Status { get; set; }
    public int GroupId { get; set; }
    public string IssuerName { get; set; } = null!;
    public string IssuerEmail { get; set; } = null!;
    public string RemovedUserName { get; set; } = null!;
    public string RemovedUserEmail { get; set; } = null!;
}

public class GroupLeftEventArgs
{
    public bool Status { get; set; }
    public int GroupId { get; set; }
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
}

public class MessageDeliveredEventArgs
{
    public bool Status { get; set; }
    public int MessageId { get; set; }
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public int ConversationId { get; set; }
}

public class OnlineStatusChangedEventArgs
{
    public bool Status { get; set; }
    public int MessageId { get; set; }
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public int ConversationId { get; set; }
    public bool Online { get; set; }
}

public class ConversationOpenedEventArgs
{
    public bool Status { get; set; }
    public int ConversationId { get; set; }
}
