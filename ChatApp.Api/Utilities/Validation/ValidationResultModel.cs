namespace ChatApp.Api.Utilities.Validation;

public class ValidationResultModel
{
    public string Message { get; }
    public string[] Errors { get; }
    public ValidationResultModel(ModelStateDictionary modelState)
    {
        Message = "Validation Failed";
        Errors = modelState.Keys
                .SelectMany(key => modelState[key]?.Errors.Select(x => x.ErrorMessage) ?? null!)
                ?.ToArray() ?? null!;
    }
}
