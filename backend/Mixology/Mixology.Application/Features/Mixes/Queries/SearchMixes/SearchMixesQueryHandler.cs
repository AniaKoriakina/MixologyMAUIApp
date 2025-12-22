using Mixology.Application.Cqs;
using Mixology.Application.Features.Mixes.Queries.Dto;
using Mixology.Application.Features.Mixes.Queries.Helpers;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.Mixes.Queries.SearchMixes;

public class SearchMixesQueryHandler : PagedQueryHandler<SearchMixesQuery, MixDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public SearchMixesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<PagedResult<MixDto>>> Handle(SearchMixesQuery request, CancellationToken cancellationToken)
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

        var totalCount = mixes.Count();
        var pagedMixes = mixes.Skip(request.Skip).Take(request.Take).ToList();
        
        var allTobaccoIds = pagedMixes
            .SelectMany(m => m.Compositions.Select(c => c.TobaccoId))
            .Distinct()
            .ToList();
        
        var tobaccos = await _unitOfWork.RawMaterials.GetByIdsAsync(allTobaccoIds);
        var tobaccoDict = tobaccos.ToDictionary(t => t.Id, t => t.Name);

        var result = new List<MixDto>();
        
        foreach (var mix in pagedMixes)
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

        var pagedResult = new PagedResult<MixDto>(result, totalCount, request.Page, request.PageSize);
        return Success(pagedResult);
    }
}
