namespace Mixology.Core.Shared.Result;

public interface IResult<out T> : IResult
{
    T Value { get; }
}