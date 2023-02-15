namespace ChatApp.Api.Dtos.Responses;

public static class Handlers
{
    public static BaseResponseDto MyOk(string Message, params string[] errors)
    {
        return new BaseResponseDto()
        {
            Message = Message,
            Status = true,
            StatusCode = StatusCodes.Status200OK
        };
    }

    public static BaseResponseDto<T> MyOk<T>(T data, string Message = "", params string[] errors)
    {
        return new BaseResponseDto<T>()
        {
            Data = data,
            Message = string.IsNullOrEmpty(Message) ? "Success" : Message,
            Status = true,
            StatusCode = StatusCodes.Status200OK
        };
    }

    public static BaseResponseDto MyBadRequest(string Message, params string[] errors)
    {
        return new BaseResponseDto()
        {
            Message = Message,
            Status = false,
            Errors = errors,
            StatusCode = StatusCodes.Status400BadRequest
        };
    }

    public static BaseResponseDto MyUnauthorized(string Message, params string[] errors)
    {
        return new BaseResponseDto()
        {
            Message = Message,
            Status = false,
            Errors = errors,
            StatusCode = StatusCodes.Status401Unauthorized
        };
    }


    public static BaseResponseDto MyForbidden(string Message, params string[] errors)
    {
        return new BaseResponseDto()
        {
            Message = Message,
            Status = false,
            Errors = errors,
            StatusCode = StatusCodes.Status403Forbidden
        };
    }
}
