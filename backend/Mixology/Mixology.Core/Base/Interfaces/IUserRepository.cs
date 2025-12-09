using Mixology.Core.Entities;

namespace Mixology.Core.Base.Infrastructure;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByEmail(string email);
    Task<IEnumerable<User>> GetActiveUsersAsync();
}