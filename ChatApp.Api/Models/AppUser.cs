namespace ChatApp.Api.Models;

public class AppUser : IdentityUser
{
    public int MyProperty { get; set; }
    public ICollection<Group>? Groups { get; set; }
    public ICollection<Conversation>? Conversations { get; set; }
    public DateTime? LastTimeOnline { get; set; }
}
