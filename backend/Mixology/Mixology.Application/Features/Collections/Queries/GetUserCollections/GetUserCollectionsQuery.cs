using Mixology.Application.Cqs;
using Mixology.Application.Features.Collections.Queries.Dto;

namespace Mixology.Application.Features.Collections.Queries.GetUserCollections;

public class GetUserCollectionsQuery : Query<List<CollectionDto>>
{
    public long UserId { get; set; }
}
