namespace ChatApp.Api.Dtos;

public class MessageDto
{
    public int Id { get; set; }
    public string FromUserId { get; set; } = null!;
    public string ToUserId { get; set; } = null!;
    public UserDto FromUser { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int? GroupId { get; set; }
    public GroupDto? Group { get; set; }
}
