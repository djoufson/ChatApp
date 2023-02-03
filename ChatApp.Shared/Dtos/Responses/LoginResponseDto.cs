namespace ChatApp.Shared.Dtos.Responses
{
    public class LoginResponseDto : BaseResponseDto
    {
        public UserWithoutEntities User { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
