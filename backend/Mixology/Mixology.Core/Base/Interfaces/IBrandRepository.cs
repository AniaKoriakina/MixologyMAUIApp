using Mixology.Core.Entities;

namespace Mixology.Core.Base.Infrastructure;

public interface IBrandRepository : IRepository<Brand>
{
    Task<Brand?> GetByNameAsync(string name);
    Task<IEnumerable<Brand>> SearchByNameAsync(string searchTerm);
}
