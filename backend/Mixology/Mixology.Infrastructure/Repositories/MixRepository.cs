using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Mixology.Infrastructure.Data;

namespace Mixology.Infrastructure.Repositories;

public class MixRepository : Repository<Mix>, IMixRepository
{
    public MixRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Mix>> GetByUserIdAsync(long userId)
    {
        return await _dbSet
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Mix>> GetTopRatedAsync(int count)
    {
        return await _dbSet
            .OrderByDescending(m => m.RatingAverage)
            .Take(count)
            .ToListAsync();
    }
}