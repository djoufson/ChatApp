namespace ChatApp.Shared.Dtos.Requests;

public class SendMessageToGroupDto
{
    public int? QueueId { get; set; }
    [Required]
    public string Content { get; set; } = null!;
}
