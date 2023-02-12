namespace ChatApp.Api.Models;

public class GroupConnection
{
    [Key, Required]
    public string Id { get; set; } = null!;
    [Required]
    public string GroupId { get; set; } = null!;
}
