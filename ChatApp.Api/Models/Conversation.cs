namespace ChatApp.Api.Models;

public class Conversation
{
    [Key] public int Id { get; set; }
    public ICollection<Message> Messages { get; set; } = null!;
}
