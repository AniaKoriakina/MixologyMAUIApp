using Mixology.Application.Cqs;
using Mixology.Application.Features.Mixes.Queries.Dto;
using Mixology.Application.Features.Mixes.Queries.Helpers;
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
        
        var allTobaccoIds = favoriteMixes
            .SelectMany(m => m.Compositions.Select(c => c.TobaccoId))
            .Distinct()
            .ToList();
        
        var tobaccos = await _unitOfWork.RawMaterials.GetByIdsAsync(allTobaccoIds);
        var tobaccoDict = tobaccos.ToDictionary(t => t.Id, t => t.Name);
        
        var result = new List<MixDto>();
        
        foreach (var mix in favoriteMixes)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(mix.UserId);
            var authorName = user?.IsActive == true ? user.Username : "Удалённый пользователь";
            var ratingCount = await _unitOfWork.Ratings.GetRatingCountForMixAsync(mix.Id);
            
            var compositionDtos = mix.Compositions.Select(composition => new MixCompositionDto
            {
                TobaccoId = composition.TobaccoId,
                TobaccoName = tobaccoDict.GetValueOrDefault(composition.TobaccoId, "Неизвестно"),
                Percentage = composition.Percentage
            }).ToList();

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
                Compositions = compositionDtos
            });
        }

        return Success(result);
    }
}
