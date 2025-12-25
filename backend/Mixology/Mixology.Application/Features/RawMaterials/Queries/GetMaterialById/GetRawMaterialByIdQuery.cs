using Mixology.Application.Cqs;
using Mixology.Application.Features.RawMaterials.Queries.Dto;

namespace Mixology.Application.Features.RawMaterials.Queries.GetMaterialById;

public class GetRawMaterialByIdQuery : Query<RawMaterialDto>
{
    public long Id { get; set; }
}