using Mixology.Core.Base;

namespace Mixology.Core.Entities;

public class FavoriteMix : BaseEntity
{
    public long UserId { get; set; }
    public long MixId { get; set; }
}