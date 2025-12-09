using Mixology.Application.Cqs;
using Mixology.Application.Features.Mixes.Queries.Dto;

namespace Mixology.Application.Features.FavoriteMixes.Queries.GetFavoriteMixes;

public class GetFavoriteMixesQuery : Query<List<MixDto>>
{
    public long UserId { get; set; }
}
