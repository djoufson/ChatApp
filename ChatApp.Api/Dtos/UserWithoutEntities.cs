namespace ChatApp.Api.Dtos;

public class UserWithoutEntities
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime? LastTimeOnline { get; set; }
    public ICollection<ConversationWithoutEntities>? Conversations { get; set; }
}
