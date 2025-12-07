namespace Mixology.Core.Shared.Result;

public class ValidationError : Error
{
    public override string Type => "ValidationError";
    
    public string Message { get; set; }

    public ValidationError(string? message)
    {
        Message = message;
    }

    public static ValidationError Create(string message)
    {
        return new ValidationError(message);
    }
}