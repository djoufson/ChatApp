namespace ChatApp.Api.Dtos;

public class GroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<UserDto> Members { get; set; } = null!;
    public ICollection<MessageDto> Messages { get; set; } = null!;
}
