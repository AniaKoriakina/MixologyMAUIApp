namespace Mixology.Core.Shared.Result;

public interface IResult
{
    bool IsSuccess { get; }
    IReadOnlyList<IError> GetErrors();
}