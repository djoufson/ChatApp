namespace ChatApp.Shared.Dtos.Requests;

public class RegisterDto
{
    [Required(ErrorMessage = "A Username is required")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "The email Address is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "A password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "The confirmation pasword is required")]
    [Compare(nameof(Password), ErrorMessage = "The password does not match")]
    public string PasswordConfirm { get; set; } = null!;

    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; } = null!;
}
