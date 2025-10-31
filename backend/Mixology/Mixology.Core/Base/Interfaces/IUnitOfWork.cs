using Mixology.Core.Entities;

namespace Mixology.Core.Base.Infrastructure;

public interface IUnitOfWork : IDisposable
{
    IMixRepository Mixes { get; }
    IRepository<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}