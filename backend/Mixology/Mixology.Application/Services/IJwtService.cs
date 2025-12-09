using Mixology.Core.Entities;

namespace Mixology.Application.Services;

public interface IJwtService
{
    string GenerateToken(User user);
    long? ValidateToken(string token);
}
