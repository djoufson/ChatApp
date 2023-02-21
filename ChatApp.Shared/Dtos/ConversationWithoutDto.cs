namespace ChatApp.Shared.Dtos;

public class ConversationWithoutEntities
{
    public int Id { get; set; }
    public ICollection<MessageWithoutEntities> Messages { get; set; } = null!;
    public int UnreadMessagesCount { get; set; }
}