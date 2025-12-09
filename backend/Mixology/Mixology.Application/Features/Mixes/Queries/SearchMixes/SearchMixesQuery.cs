using Mixology.Application.Cqs;
using Mixology.Application.Features.Mixes.Queries.Dto;

namespace Mixology.Application.Features.Mixes.Queries.SearchMixes;

public class SearchMixesQuery : Query<List<MixDto>>
{
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; } // "rating_asc", "rating_desc", "newest"
}
