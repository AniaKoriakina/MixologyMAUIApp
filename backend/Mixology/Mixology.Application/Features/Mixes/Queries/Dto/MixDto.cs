using Mixology.Core.ValueObjects;

namespace Mixology.Application.Features.Mixes.Queries.Dto;

public class MixDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; } 
    public bool HasImage { get; set; }
    public string AuthorName { get; set; }
    public double RatingAverage { get; set; }
    public int RatingCount { get; set; }
    public FlavorProfile Flavor { get; set; }
    public List<MixComposition> Compositions { get; set; }
    public string? CollectionName { get; set; }
}
