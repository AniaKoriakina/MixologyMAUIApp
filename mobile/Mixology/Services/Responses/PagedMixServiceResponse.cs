using Mixology.Services.DTOs;

namespace Mixology.Services.Responses;

public class PagedMixServiceResponse
{
    public bool IsSuccess { get; set; }
    public PagedResult<MixDto>? Value { get; set; }
    public string? ErrorMessage { get; set; }
}