namespace ChatApp.Shared.Dtos;

public class ConversationDto
{
    public int Id { get; set; }
    public ICollection<MessageDto> Messages { get; set; } = null!;
}