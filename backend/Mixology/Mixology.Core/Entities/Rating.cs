using Mixology.Core.Base;
using Mixology.Core.ValueObjects;

namespace Mixology.Core.Entities;

public class Rating : BaseEntity
{
    public long UserId { get; set; }
    public long MixId { get; set; }
    public RatingValue Value { get; set; }
}