using Mixology.Application.Cqs;
using Mixology.Application.Features.Mixes.Queries.Dto;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.FavoriteMixes.Queries.GetFavoriteMixes;

public class GetFavoriteMixesQueryHandler : QueryHandler<GetFavoriteMixesQuery, List<MixDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFavoriteMixesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<List<MixDto>>> Handle(GetFavoriteMixesQuery request, CancellationToken cancellationToken)
    {
        var favoriteMixes = await _unitOfWork.FavoriteMixes.GetFavoriteMixesByUserIdAsync(request.UserId);
        
        var result = new List<MixDto>();
        
        foreach (var mix in favoriteMixes)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(mix.UserId);
            var authorName = user?.IsActive == true ? user.Username : "Удалённый пользователь";
            var ratingCount = await _unitOfWork.Ratings.GetRatingCountForMixAsync(mix.Id);

            result.Add(new MixDto
            {
                Id = mix.Id,
                Name = mix.Name,
                Description = mix.Description,
                ImageUrl = mix.ImageData != null ? $"/api/images/mix/{mix.Id}" : null,
                HasImage = mix.ImageData != null,
                AuthorName = authorName,
                RatingAverage = mix.RatingAverage,
                RatingCount = ratingCount,
                Flavor = mix.Flavor,
                Compositions = mix.Compositions
            });
        }

        return Success(result);
    }
}
