namespace ChatApp.Api.Dtos.Requests;

public class LoginDto
{
    [Required(ErrorMessage = "The Email field is required")]
    [DataType(DataType.EmailAddress, ErrorMessage = "The email address is not valid")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "The password field is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
