using Mixology.Application.Cqs;
using Mixology.Application.Features.Auth.Dto;

namespace Mixology.Application.Features.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQuery : Query<UserDto>
{
    public long UserId { get; set; }
}
