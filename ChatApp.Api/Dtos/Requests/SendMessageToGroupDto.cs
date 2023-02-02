namespace ChatApp.Api.Dtos.Requests;

public class SendMessageToGroupDto
{
    [Required]
    public string Content { get; set; } = null!;
}
