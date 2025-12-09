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
        return await _dbSet
            .Where(rm => rm.Name.Contains(searchTerm) || 
                        rm.Flavor.Tags.Any(t => t.Contains(searchTerm)))
            .ToListAsync();
    }
}
