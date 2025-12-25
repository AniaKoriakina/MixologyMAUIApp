namespace Mixology.Services.DTOs;

public class RawMaterialDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public FlavorProfile Flavor { get; set; } 
    public int Strength {get; set;}
    public long BrandId {get; set;}
}