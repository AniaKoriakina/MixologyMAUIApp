using Mixology.Core.Entities;

namespace Mixology.Core.Base.Infrastructure;

public interface ICollectionRepository : IRepository<Collection>
{
    Task<IEnumerable<Collection>> GetByUserIdAsync(long userId);
    Task<Collection?> GetDefaultCollectionAsync(long userId);
    Task<IEnumerable<Collection>> GetCollectionsWithMixesAsync(long userId);
}
