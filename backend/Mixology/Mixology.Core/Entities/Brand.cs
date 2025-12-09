using Mixology.Core.Base;

namespace Mixology.Core.Entities;

public class Brand : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[]? LogoData { get; set; }
    public string? LogoContentType { get; set; }
    public string? LogoFileName { get; set; }
    public List<RawMaterial> RawMaterials { get; set; } = new();
}