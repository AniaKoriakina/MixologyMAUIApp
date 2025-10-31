using Mixology.Core.Base;
using Mixology.Core.Enum;

namespace Mixology.Core.Entities;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    public bool IsActive { get; set; }
    
    public List<Mix> Mixes { get; set; } = new List<Mix>();
    
}