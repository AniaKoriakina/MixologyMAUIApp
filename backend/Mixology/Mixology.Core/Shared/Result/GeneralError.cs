namespace Mixology.Core.Shared.Result;

public class GeneralError : Error
{
    public override string Type => "GeneralError";
    
    public string Message { get; set; }

    public GeneralError(string message)
    {
        Message = message;
    }
}
