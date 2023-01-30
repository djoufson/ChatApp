namespace ChatApp.Api.Dtos.Requests;

public class EmailDto
{
    [Required(ErrorMessage = "The email field is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
}
