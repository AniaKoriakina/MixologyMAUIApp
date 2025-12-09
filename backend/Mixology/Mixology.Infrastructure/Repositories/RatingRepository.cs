using Microsoft.EntityFrameworkCore;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Entities;
using Mixology.Infrastructure.Data;

namespace Mixology.Infrastructure.Repositories;

public class RatingRepository : Repository<Rating>, IRatingRepository
{
    public RatingRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Rating?> GetUserRatingForMixAsync(long userId, long mixId)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.UserId == userId && r.MixId == mixId);
    }

    public async Task<double> GetAverageRatingForMixAsync(long mixId)
    {
        var ratings = await _dbSet.Where(r => r.MixId == mixId).ToListAsync();
        return ratings.Any() ? ratings.Average(r => r.Value.Value) : 0;
    }

    public async Task<int> GetRatingCountForMixAsync(long mixId)
    {
        return await _dbSet.CountAsync(r => r.MixId == mixId);
    }
}
