using Mixology.Application.Cqs;
using Mixology.Application.Features.Mixes.Queries.Dto;
using Mixology.Application.Features.Mixes.Queries.Helpers;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.Mixes.Queries.GetMixById;

public class GetMixByIdQueryHandler : QueryHandler<GetMixByIdQuery, MixDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMixByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<MixDto>> Handle(GetMixByIdQuery request, CancellationToken cancellationToken)
    {
        var mix = await _unitOfWork.Mixes.GetByIdAsync(request.MixId);
        if (mix == null)
        {
            return Error(new GeneralError("Mix not found"));
        }

        var user = await _unitOfWork.Users.GetByIdAsync(mix.UserId);
        var authorName = user?.IsActive == true ? user.Username : "Удалённый пользователь";

        var ratingCount = await _unitOfWork.Ratings.GetRatingCountForMixAsync(mix.Id);

        string? collectionName = null;
        if (mix.CollectionId.HasValue)
        {
            var collection = await _unitOfWork.Collections.GetByIdAsync(mix.CollectionId.Value);
            if (collection != null && !collection.IsDefault)
            {
                collectionName = collection.Name;
            }
        }
        
        var compositionDtos = await MixMappingHelper.MapCompositionsAsync(mix.Compositions, _unitOfWork);

        var dto = new MixDto
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
            Compositions = compositionDtos,
            CollectionName = collectionName
        };

        return Success(dto);
    }
}
