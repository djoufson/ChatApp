namespace ChatApp.Api.Dtos;

public class UserDto
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public ICollection<MessageDto>? Messages { get; set; }
    public ICollection<GroupDto>? Groups { get; set; }
    public DateTime? LastTimeOnline { get; set; }
}
