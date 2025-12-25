using System.Text.Json.Serialization;

namespace Mixology.Services.DTOs;

public class MixDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool HasImage { get; set; }
    public string AuthorName { get; set; }
    public float RatingAverage { get; set; }
    public int RatingCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public FlavorProfile? Flavor { get; set; }
    
    [JsonIgnore]
    public List<string> FlavorTags => Flavor?.Tags ?? new List<string>();
    
    public List<CompositionDto> Compositions { get; set; } = new List<CompositionDto>();
    public string? CollectionName { get; set; }
}