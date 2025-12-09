using Mixology.Core.Entities;

namespace Mixology.Core.Base.Infrastructure;

public interface IRatingRepository : IRepository<Rating>
{
    Task<Rating?> GetUserRatingForMixAsync(long userId, long mixId);
    Task<double> GetAverageRatingForMixAsync(long mixId);
    Task<int> GetRatingCountForMixAsync(long mixId);
}
