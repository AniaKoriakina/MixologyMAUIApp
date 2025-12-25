using Microsoft.EntityFrameworkCore;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Entities;
using Mixology.Infrastructure.Data;

namespace Mixology.Infrastructure.Repositories;

public class RawMaterialRepository : Repository<RawMaterial>, IRawMaterialRepository
{
    public RawMaterialRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<RawMaterial>> GetByBrandIdAsync(long brandId)
    {
        return await _dbSet
            .Where(rm => rm.BrandId == brandId)
            .ToListAsync();
    }

    public async Task<IEnumerable<RawMaterial>> SearchByNameOrFlavorAsync(string searchTerm)
    {
        var materials = await _dbSet.ToListAsync();
        
        return materials.Where(rm =>
            rm.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            rm.Flavor.Tags.Any(t => t.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
        );
    }
    
    public async Task<IEnumerable<RawMaterial>> GetByIdsAsync(IEnumerable<long> ids)
    {
        return await _dbSet
            .Where(rm => ids.Contains(rm.Id))
            .ToListAsync();
    }
}
