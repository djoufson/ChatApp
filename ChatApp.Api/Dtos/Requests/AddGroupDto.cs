namespace ChatApp.Api.Dtos.Requests;

public class AddGroupDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public IEnumerable<string> MembersMailAddresses { get; set; } = null!;
}
