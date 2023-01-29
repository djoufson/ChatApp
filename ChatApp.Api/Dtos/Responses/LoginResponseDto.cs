namespace ChatApp.Api.Dtos.Responses
{
    public class LoginResponseDto : BaseResponseDto
    {
        public UserDto User { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
