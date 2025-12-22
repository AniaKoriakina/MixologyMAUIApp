using Mixology.Core.Entities;

namespace Mixology.Core.Base.Infrastructure;

public interface IRawMaterialRepository : IRepository<RawMaterial>
{
    Task<IEnumerable<RawMaterial>> GetByBrandIdAsync(long brandId);
    Task<IEnumerable<RawMaterial>> SearchByNameOrFlavorAsync(string searchTerm);
    Task<IEnumerable<RawMaterial>> GetByIdsAsync(IEnumerable<long> ids);
}
