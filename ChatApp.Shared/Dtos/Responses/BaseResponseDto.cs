namespace ChatApp.Shared.Dtos.Responses;

public class BaseResponseDto
{
    public bool Status { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string[]? Errors { get; set; }
}

public class BaseResponseDto<T>
{
    public T? Data { get; set; }
    public bool Status { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string[]? Errors { get; set; }
}
