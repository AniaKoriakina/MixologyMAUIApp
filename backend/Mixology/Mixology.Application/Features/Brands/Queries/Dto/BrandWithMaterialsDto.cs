using Mixology.Application.Features.RawMaterials.Queries.Dto;

namespace Mixology.Application.Features.Brands.Queries.Dto;

public class BrandWithMaterialsDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? LogoUrl { get; set; }
    public bool HasLogo { get; set; }
    public List<RawMaterialDto> RawMaterials { get; set; } = [];
}
