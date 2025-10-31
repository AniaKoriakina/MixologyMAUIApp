using Mixology.Core.Entities;

namespace Mixology.Core.Base.Infrastructure;

public interface IMixRepository : IRepository<Mix>
{
    Task<IEnumerable<Mix>> GetByUserIdAsync(long userId);
    Task<IEnumerable<Mix>> GetTopRatedAsync(int count);
}