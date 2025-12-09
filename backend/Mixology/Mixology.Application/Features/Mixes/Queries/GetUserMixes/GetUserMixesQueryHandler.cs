using Mixology.Application.Cqs;
using Mixology.Application.Features.Mixes.Queries.Dto;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.Mixes.Queries.GetUserMixes;

public class GetUserMixesQueryHandler : QueryHandler<GetUserMixesQuery, List<MixDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserMixesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<List<MixDto>>> Handle(GetUserMixesQuery request, CancellationToken cancellationToken)
    {
        var userMixes = await _unitOfWork.Mixes.GetByUserIdAsync(request.UserId);
        var mixes = userMixes.AsEnumerable();
        
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            mixes = mixes.Where(m =>
                m.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                m.Flavor.Tags.Any(t => t.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
            );
        }

        var result = new List<MixDto>();
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
        var authorName = user?.IsActive == true ? user.Username : "Удалённый пользователь";

        foreach (var mix in mixes)
        {
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
