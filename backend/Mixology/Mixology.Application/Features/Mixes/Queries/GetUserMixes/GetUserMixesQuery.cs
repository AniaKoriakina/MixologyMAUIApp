using Mixology.Application.Cqs;
using Mixology.Application.Features.Mixes.Queries.Dto;

namespace Mixology.Application.Features.Mixes.Queries.GetUserMixes;

public class GetUserMixesQuery : Query<List<MixDto>>
{
    public long UserId { get; set; }
    public string? SearchTerm { get; set; }
}
