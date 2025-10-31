using Mixology.Core.Base;

namespace Mixology.Core.Entities;

public class Collection : BaseEntity
{
    public string Name { get; set; }
    public string? Image { get; set; }
    public long UserId { get; set; }
    public bool IsDefault { get; set; }
    
    public List<Mix> Mixes { get; set; }
}