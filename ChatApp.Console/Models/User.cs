namespace ChatApp.Console.Models;

public class User
{
    public string Id { get; set; } = null!; 
    public string Name { get; set; } = null!;
    public List<Conversation> Conversations { get; set; } = null!;
}
