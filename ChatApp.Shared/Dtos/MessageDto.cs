namespace ChatApp.Shared.Dtos;

public class MessageDto
{
    public int Id { get; set; }
    public string FromUserId { get; set; } = null!;
    public string FromUserName { get; set; } = null!;
    public string FromUserEmail { get; set; } = null!;
    public string? ToUserId { get; set; }
    public string? ToUserEmail { get; set; }
    public string? ToUserName { get; set; }
    public string Content { get; set; } = null!;
    public int? ConversationId { get; set; }
    public ConversationDto? Conversation { get; set; }
    public int? GroupId { get; set; }
    public GroupDto? Group { get; set; }
    public DateTime SentAt { get; set; }
    public MessageDto? Queue { get; set; }
    public int? QueueId { get; set; }
}
