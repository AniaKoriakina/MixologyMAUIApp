using Mixology.Application.Cqs;
using Mixology.Application.Features.Brands.Queries.Dto;

namespace Mixology.Application.Features.Brands.Queries.GetAllBrands;

public class GetAllBrandsQuery : Query<List<BrandDto>>
{
    public string? SearchTerm { get; set; }
}
