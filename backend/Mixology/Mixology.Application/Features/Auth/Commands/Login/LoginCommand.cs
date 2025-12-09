using Mixology.Application.Cqs;
using Mixology.Application.Features.Auth.Dto;

namespace Mixology.Application.Features.Auth.Commands.Login;

public class LoginCommand : Query<AuthResponseDto>
{
    public string Email { get; set; } 
    public string Password { get; set; }
}
