using Microsoft.EntityFrameworkCore;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Entities;
using Mixology.Infrastructure.Data;

namespace Mixology.Infrastructure.Repositories;

public class BrandRepository : Repository<Brand>, IBrandRepository
{
    public BrandRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Brand?> GetByNameAsync(string name)
    {
        return await _dbSet
            .Include(b => b.RawMaterials)
            .FirstOrDefaultAsync(b => b.Name == name);
    }

    public async Task<IEnumerable<Brand>> SearchByNameAsync(string searchTerm)
    {
        return await _dbSet
            .Where(b => b.Name.Contains(searchTerm))
            .Include(b => b.RawMaterials)
            .ToListAsync();
    }
}
