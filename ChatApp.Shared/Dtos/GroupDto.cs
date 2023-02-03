namespace ChatApp.Shared.Dtos;

public class GroupDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public ICollection<UserDto> Members { get; set; } = null!;
    public ICollection<MessageDto>? Messages { get; set; } = null!;
}
