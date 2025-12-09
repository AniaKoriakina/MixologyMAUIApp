using Mixology.Application.Cqs;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Entities;
using Mixology.Core.Shared.Result;
using Mixology.Core.ValueObjects;

namespace Mixology.Application.Features.Ratings.Commands.RateMix;

public class RateMixCommandHandler : CommandHandler<RateMixCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RateMixCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result> Handle(RateMixCommand request, CancellationToken cancellationToken)
    {
        var mix = await _unitOfWork.Mixes.GetByIdAsync(request.MixId);
        if (mix == null)
        {
            return Error(new GeneralError("Mix not found"));
        }

        try
        {
            var ratingValue = new RatingValue(request.RatingValue);
            
            var existingRating = await _unitOfWork.Ratings.GetUserRatingForMixAsync(request.UserId, request.MixId);
            
            if (existingRating != null)
            {
                existingRating.Value = ratingValue;
                await _unitOfWork.Ratings.UpdateAsync(existingRating);
            }
            else
            {
                var rating = new Rating
                {
                    UserId = request.UserId,
                    MixId = request.MixId,
                    Value = ratingValue
                };
                await _unitOfWork.Ratings.AddAsync(rating);
            }
            
            var averageRating = await _unitOfWork.Ratings.GetAverageRatingForMixAsync(request.MixId);
            mix.RatingAverage = averageRating;
            
            await _unitOfWork.Mixes.UpdateAsync(mix);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Success();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return Error(new ValidationError(ex.Message));
        }
    }
}
