using Mixology.Services.DTOs;

namespace Mixology.Services.Responses;

public class MaterialServiceResponse
{
    public RawMaterialDto Value { get; set; }
    public bool IsSuccess { get; set; }
}