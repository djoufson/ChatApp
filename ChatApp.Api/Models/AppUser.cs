namespace ChatApp.Api.Models;

public class AppUser : IdentityUser
{
    public ICollection<Message>? Messages { get; set; }
    public ICollection<Group>? Groups { get; set; }
    public DateTime? LastTimeOnline { get; set; }
}
