namespace ChatApp.Api.Dtos.Responses;

public class BaseResponseDto
{
    public bool Status { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
}
