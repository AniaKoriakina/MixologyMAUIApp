using Mixology.Services.DTOs;

namespace Mixology.Services.Responses;

public class MixServiceResponse
{
    public List<MixDto> Value { get; set; }
    public bool IsSuccess { get; set; }
}