using Mixology.Core.Entities;

namespace Mixology.Core.Base.Infrastructure;

public interface IUnitOfWork : IDisposable
{
    IMixRepository Mixes { get; }
    IUserRepository Users { get; }
    IBrandRepository Brands { get; }
    IRawMaterialRepository RawMaterials { get; }
    ICollectionRepository Collections { get; }
    IFavoriteMixRepository FavoriteMixes { get; }
    IRatingRepository Ratings { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}