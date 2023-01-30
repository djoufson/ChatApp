namespace ChatApp.Api.Models;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<AppUser> Members { get; set; } = null!;
    public ICollection<Message>? Messages { get; set; } = null!;
}
