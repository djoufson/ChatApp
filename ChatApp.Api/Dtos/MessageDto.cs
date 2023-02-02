namespace ChatApp.Api.Dtos;

public class MessageDto
{
    public int Id { get; set; }
    public string FromUserId { get; set; } = null!;
    public string? ToUserId { get; set; }
    public string Content { get; set; } = null!;
    public int? ConversationId { get; set; }
    public Conversation? Conversation { get; set; }
    public int? GroupId { get; set; }
    public GroupDto? Group { get; set; }
    public DateTime SentAt { get; set; }
}
