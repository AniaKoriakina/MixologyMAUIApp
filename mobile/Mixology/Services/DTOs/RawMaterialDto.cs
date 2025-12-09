namespace Mixology.Services.DTOs;

public class RawMaterialDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<FlavorDto> Flavors { get; set; } = new List<FlavorDto>();
    public int Strength {get; set;}
    public long BrandId {get; set;}
}