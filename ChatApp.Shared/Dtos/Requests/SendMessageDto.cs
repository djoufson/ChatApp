namespace ChatApp.Shared.Dtos.Requests;

public class SendMessageDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string ToUserMail { get; set; } = null!;

    [Required]
    public string Content { get; set; } = null!;
}
