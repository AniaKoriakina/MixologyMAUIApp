using Mixology.Application.Cqs;

namespace Mixology.Application.Features.Ratings.Commands.RateMix;

public class RateMixCommand : Command
{
    public long UserId { get; set; }
    public long MixId { get; set; }
    public int RatingValue { get; set; } // 1-5
}
