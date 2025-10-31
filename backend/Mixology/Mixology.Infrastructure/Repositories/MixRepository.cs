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

    public Task<IEnumerable<Mix>> GetByUserIdAsync(long userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Mix>> GetTopRatedAsync(int count)
    {
        throw new NotImplementedException();
    }
}