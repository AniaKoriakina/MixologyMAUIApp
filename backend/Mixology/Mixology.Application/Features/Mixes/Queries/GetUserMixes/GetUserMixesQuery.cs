using Mixology.Application.Cqs;
using Mixology.Application.Features.Mixes.Queries.Dto;

namespace Mixology.Application.Features.Mixes.Queries.GetUserMixes;

public class GetUserMixesQuery : PagedQuery<MixDto>
{
    public long UserId { get; set; }
    public string? SearchTerm { get; set; }
}
