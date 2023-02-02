namespace ChatApp.Api.Dtos;

public class ConversationDto
{
    public int Id { get; set; }
    public ICollection<MessageDto> Messages { get; set; } = null!;
}