namespace ChatApp.Api.Utilities.Validation;

public class ValidationError
{
    public string Field { get; }
    public int Code { get; set; }
    public string Message { get; }

    public ValidationError(string field, int code, string message)
    {
        Field = (field != string.Empty) ? field : null!;
        Code = code;
        Message = message;
    }
}

public class ValidationResultModel
{
    public string Message { get; }
    public List<ValidationError> Errors { get; }
    public ValidationResultModel(ModelStateDictionary modelState)
    {
        Message = "Validation Failed";
        Errors = modelState.Keys
                .SelectMany(key => modelState[key]?.Errors.Select(x => new ValidationError(key, 400, x.ErrorMessage)) ?? null!)
                ?.ToList() ?? null!;
    }
}
