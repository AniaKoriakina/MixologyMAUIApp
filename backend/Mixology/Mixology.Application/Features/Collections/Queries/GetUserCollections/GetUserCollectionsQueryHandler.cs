using Mixology.Application.Cqs;
using Mixology.Application.Features.Collections.Queries.Dto;
using Mixology.Application.Features.Mixes.Queries.Dto;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.Collections.Queries.GetUserCollections;

public class GetUserCollectionsQueryHandler : QueryHandler<GetUserCollectionsQuery, List<CollectionDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserCollectionsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<List<CollectionDto>>> Handle(GetUserCollectionsQuery request, CancellationToken cancellationToken)
    {
        var collections = await _unitOfWork.Collections.GetCollectionsWithMixesAsync(request.UserId);
        
        var result = new List<CollectionDto>();
        
        foreach (var collection in collections)
        {
            var topMixes = collection.Mixes
                .OrderByDescending(m => m.RatingAverage)
                .Take(3)
                .Select(m => new MixDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    ImageUrl = m.ImageData != null ? $"/api/images/mix/{m.Id}" : null,
                    HasImage = m.ImageData != null,
                    RatingAverage = m.RatingAverage,
                    Flavor = m.Flavor,
                    Compositions = m.Compositions
                })
                .ToList();

            result.Add(new CollectionDto
            {
                Id = collection.Id,
                Name = collection.Name,
                ImageUrl = collection.ImageData != null ? $"/api/images/collection/{collection.Id}" : null,
                HasImage = collection.ImageData != null,
                TopMixes = topMixes
            });
        }

        return Success(result);
    }
}
