using Mixology.Application.Cqs;
using Mixology.Application.Features.Mixes.Queries.Dto;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.Mixes.Queries.SearchMixes;

public class SearchMixesQueryHandler : QueryHandler<SearchMixesQuery, List<MixDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public SearchMixesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<List<MixDto>>> Handle(SearchMixesQuery request, CancellationToken cancellationToken)
    {
        var allMixes = await _unitOfWork.Mixes.GetAllAsync();
        var mixes = allMixes.AsEnumerable();
        
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            mixes = mixes.Where(m =>
                m.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                m.Flavor.Tags.Any(t => t.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
            );
        }
        
        mixes = request.SortBy switch
        {
            "rating_asc" => mixes.OrderBy(m => m.RatingAverage),
            "rating_desc" => mixes.OrderByDescending(m => m.RatingAverage),
            "newest" => mixes.OrderByDescending(m => m.CreatedAt),
            _ => mixes.OrderByDescending(m => m.RatingAverage)
        };

        var result = new List<MixDto>();
        
        foreach (var mix in mixes)
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
