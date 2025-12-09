using Mixology.Core.ValueObjects;

namespace Mixology.Application.Features.RawMaterials.Queries.Dto;

public class RawMaterialDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public FlavorProfile Flavor { get; set; }
    public int Strength { get; set; }
    public long BrandId { get; set; }
}
