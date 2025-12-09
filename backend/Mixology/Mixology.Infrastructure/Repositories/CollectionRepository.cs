using Microsoft.EntityFrameworkCore;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Entities;
using Mixology.Infrastructure.Data;

namespace Mixology.Infrastructure.Repositories;

public class CollectionRepository : Repository<Collection>, ICollectionRepository
{
    public CollectionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Collection>> GetByUserIdAsync(long userId)
    {
        return await _dbSet
            .Where(c => c.UserId == userId)
            .Include(c => c.Mixes)
            .ToListAsync();
    }

    public async Task<Collection?> GetDefaultCollectionAsync(long userId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.UserId == userId && c.IsDefault);
    }

    public async Task<IEnumerable<Collection>> GetCollectionsWithMixesAsync(long userId)
    {
        return await _dbSet
            .Where(c => c.UserId == userId && c.Mixes.Any())
            .Include(c => c.Mixes)
            .ToListAsync();
    }
}
