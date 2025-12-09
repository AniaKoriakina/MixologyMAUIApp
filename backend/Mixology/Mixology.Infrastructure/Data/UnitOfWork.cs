using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Entities;
using Mixology.Infrastructure.Repositories;

namespace Mixology.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IMixRepository Mixes { get; }
    public IUserRepository Users { get; }
    public IBrandRepository Brands { get; }
    public IRawMaterialRepository RawMaterials { get; }
    public ICollectionRepository Collections { get; }
    public IFavoriteMixRepository FavoriteMixes { get; }
    public IRatingRepository Ratings { get; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
        Mixes = new MixRepository(context);
        Brands = new BrandRepository(context);
        RawMaterials = new RawMaterialRepository(context);
        Collections = new CollectionRepository(context);
        FavoriteMixes = new FavoriteMixRepository(context);
        Ratings = new RatingRepository(context);
    }

    #region IDisposable Implementation
    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}