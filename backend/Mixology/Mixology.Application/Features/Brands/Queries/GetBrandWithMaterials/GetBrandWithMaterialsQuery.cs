using Mixology.Application.Cqs;
using Mixology.Application.Features.Brands.Queries.Dto;

namespace Mixology.Application.Features.Brands.Queries.GetBrandWithMaterials;

public class GetBrandWithMaterialsQuery : Query<BrandWithMaterialsDto>
{
    public long BrandId { get; set; }
    public string? SearchTerm { get; set; }
}
