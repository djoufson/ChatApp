namespace ChatApp.Api.Models;

public class ChatUserConnection
{
    [Key, Required]
    public string ConnectionId { get; set; } = null!;
    [Required]
    [DataType(DataType.EmailAddress)]
    public string UserEmail { get; set; } = null!;
}
