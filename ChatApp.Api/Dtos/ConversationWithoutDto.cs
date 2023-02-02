namespace ChatApp.Api.Dtos;

public class ConversationWithoutEntities
{
    public int Id { get; set; }
    public ICollection<MessageWithoutEntities> Messages { get; set; } = null!;
}