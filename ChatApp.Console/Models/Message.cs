namespace ChatApp.Console.Models;

public class Message
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    public string FromUserId { get; set; } = null!;
    public string ToUserId { get; set; } = null!;
    public string Content { get; set; } = null!;
}
