using Mixology.Application.Cqs;
using Mixology.Application.Features.Mixes.Queries.Dto;

namespace Mixology.Application.Features.Mixes.Queries.GetMixById;

public class GetMixByIdQuery : Query<MixDto>
{
    public long MixId { get; set; }
}
