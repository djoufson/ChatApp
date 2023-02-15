namespace ChatApp.Shared.Dtos;

public class MessageWithoutEntities
{
    public int Id { get; set; }
    public string FromUserId { get; set; } = null!;
    public string FromUserName { get; set; } = null!;
    public string FromUserEmail { get; set; } = null!;
    public string? ToUserId { get; set; }
    public string? ToUserName { get; set; } = null!;
    public string? ToUserEmail { get; set; }
    public string Content { get; set; } = null!;
    public int? ConversationId { get; set; }
    public int? GroupId { get; set; }
    public DateTime SentAt { get; set; }
    public MessageWithoutEntities? Queue { get; set; }
    public int? QueueId { get; set; }
}
