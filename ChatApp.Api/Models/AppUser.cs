using Microsoft.AspNetCore.Identity;

namespace ChatApp.Api.Models;

public class AppUser : IdentityUser
{
    public string? DeviceToken { get; set; }
    public ICollection<Message>? Messages { get; set; }
    public ICollection<Group>? Groups { get; set; }
}
