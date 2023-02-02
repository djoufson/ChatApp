namespace ChatApp.Console.Models;

public class Conversation
{
    public int Id { get; set; }
    public string User1Id { get; set; } = null!;
    public string User2Id { get; set; } = null!;
    public List<Message> Messages { get; set; } = null!;
}
