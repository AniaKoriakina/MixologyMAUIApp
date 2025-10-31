using Mixology.Core.Base;
using Mixology.Core.ValueObjects;

namespace Mixology.Core.Entities;

public class RawMaterial : BaseEntity
{
    public string Name { get; set; }
    public FlavorProfile Flavor { get; set; }
    public int Strength { get; set; } // 1-3
    public long BrandId { get; set; }
    
    public List<MixComposition> Mixes { get; set; }
}