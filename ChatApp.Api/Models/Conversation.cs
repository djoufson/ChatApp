namespace ChatApp.Api.Models;

public class Conversation
{
    [Key] public int Id { get; set; }
    public DateTime StartedAt { get; set; }
    [Range(1, 2), Required]
    public ICollection<AppUser> Participents { get; set; } = null!;
    public ICollection<Message>? Messages { get; set; } = null!;
    public int UnreadMessagesCount { get; set; }
}
