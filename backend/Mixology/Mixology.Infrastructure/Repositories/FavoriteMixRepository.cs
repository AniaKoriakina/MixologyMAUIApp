using Microsoft.EntityFrameworkCore;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Entities;
using Mixology.Infrastructure.Data;

namespace Mixology.Infrastructure.Repositories;

public class FavoriteMixRepository : Repository<FavoriteMix>, IFavoriteMixRepository
{
    public FavoriteMixRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Mix>> GetFavoriteMixesByUserIdAsync(long userId)
    {
        return await _context.FavoriteMixes
            .Where(fm => fm.UserId == userId)
            .Join(_context.Mixes,
                fm => fm.MixId,
                m => m.Id,
                (fm, m) => m)
            .ToListAsync();
    }

    public async Task<bool> IsFavoriteAsync(long userId, long mixId)
    {
        return await _dbSet.AnyAsync(fm => fm.UserId == userId && fm.MixId == mixId);
    }

    public async Task RemoveFavoriteAsync(long userId, long mixId)
    {
        var favorite = await _dbSet.FirstOrDefaultAsync(fm => fm.UserId == userId && fm.MixId == mixId);
        if (favorite != null)
            _dbSet.Remove(favorite);
    }
}
