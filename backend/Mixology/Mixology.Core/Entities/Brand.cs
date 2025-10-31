using Mixology.Core.Base;

namespace Mixology.Core.Entities;

public class Brand : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Logo { get; set; }
    public List<RawMaterial> RawMaterials { get; set; } = new();
}