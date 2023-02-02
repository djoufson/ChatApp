namespace ChatApp.Api.Models;

[Index(nameof(ToUserId))]
[Index(nameof(FromUserId))]
public class Message
{
    [Key] 
    public int Id { get; set; }
    
    [ForeignKey(nameof(AppUser))] 
    public string FromUserId { get; set; } = null!;
    [ForeignKey(nameof(AppUser))]
    public string? ToUserId { get; set; }
    public string Content { get; set; } = null!;
    [ForeignKey(nameof(Conversation))]
    public int? ConversationId { get; set; } // The id of the related conversation if exists
    public Conversation? Conversation { get; set; }

    [ForeignKey(nameof(Group))] 
    public int? GroupId { get; set; } // The id of the related group if exists
    public Group? Group { get; set; } // The related group if exists
    public DateTime SentAt { get; set; }
}
