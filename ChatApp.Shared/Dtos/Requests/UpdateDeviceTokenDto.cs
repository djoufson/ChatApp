namespace ChatApp.Shared.Dtos.Requests;

public class UpdateDeviceTokenDto
{
    [Required]
    public string DeviceToken { get; set; } = null!;
}
