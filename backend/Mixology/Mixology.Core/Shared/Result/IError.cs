namespace Mixology.Core.Shared.Result;

public interface IError
{
    string Type { get; }
    Dictionary<string, object> Data { get; }
}