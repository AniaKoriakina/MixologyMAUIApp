namespace Mixology.Services.Responses.Base;

public class ApiResponse<T>
{
    public T Value { get; set; }
    public bool IsSuccess { get; set; }
}