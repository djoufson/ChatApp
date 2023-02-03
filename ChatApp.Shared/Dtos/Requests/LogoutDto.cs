namespace ChatApp.Shared.Dtos.Requests;

public class LogoutDto
{
    [Required(ErrorMessage = "The password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
