using Mixology.Core.Entities;

namespace Mixology.Core.Base.Infrastructure;

public interface IFavoriteMixRepository : IRepository<FavoriteMix>
{
    Task<IEnumerable<Mix>> GetFavoriteMixesByUserIdAsync(long userId);
    Task<bool> IsFavoriteAsync(long userId, long mixId);
    Task RemoveFavoriteAsync(long userId, long mixId);
}
