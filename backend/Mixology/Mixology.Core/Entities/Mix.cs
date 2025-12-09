using Mixology.Core.Base;
using Mixology.Core.ValueObjects;

namespace Mixology.Core.Entities;

public class Mix : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[]? ImageData { get; set; }
    public string? ImageContentType { get; set; }
    public string? ImageFileName { get; set; }
    public long UserId { get; set; }
    public long? CollectionId { get; set; }
    public double RatingAverage { get; set; }
    public FlavorProfile Flavor { get; set; }
    public List<MixComposition> Compositions { get; set; } = [];
}