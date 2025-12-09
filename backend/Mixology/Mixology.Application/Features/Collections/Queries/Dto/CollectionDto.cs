using Mixology.Application.Features.Mixes.Queries.Dto;

namespace Mixology.Application.Features.Collections.Queries.Dto;

public class CollectionDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? ImageUrl { get; set; }
    public bool HasImage { get; set; }
    public List<MixDto> TopMixes { get; set; } = [];
}
