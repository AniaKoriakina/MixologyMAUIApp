using Mixology.Application.Cqs;

namespace Mixology.Application.Features.Auth.Commands.Register;

public class RegisterCommand : Command
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
