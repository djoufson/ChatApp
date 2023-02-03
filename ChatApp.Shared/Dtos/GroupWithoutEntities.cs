namespace ChatApp.Shared.Dtos;

public class GroupWithoutEntities
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public ICollection<UserWithoutEntities> Members { get; set; } = null!;
    public ICollection<MessageWithoutEntities>? Messages { get; set; } = null!;
}
