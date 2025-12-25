using Mixology.Application.Cqs;
using Mixology.Application.Features.RawMaterials.Queries.Dto;

namespace Mixology.Application.Features.RawMaterials.Queries.GetAllMaterials;

public class GetAllMaterialsQuery : Query<List<RawMaterialDto>>
{
    public string? SearchTerm { get; set; }
}