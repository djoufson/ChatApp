using ChatApp.Shared.Dtos;

namespace ChatApp.Shared.Utilities.EventArgs;

public class RecievedMessageEventArg
{
    public bool Status { get; set; }
    public string IssuerName { get; set; } = null!;
    public string IssuerEmail { get; set; } = null!;
    public MessageWithoutEntities Message { get; set; } = null!;
}
