using Mixology.Application.Cqs;
using Mixology.Application.Features.Auth.Dto;
using Mixology.Application.Services;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : QueryHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public override async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetUserByEmail(request.Email);
        if (user == null)
        {
            return Error(new ValidationError("Неверный email или пароль"));
        }
        
        if (!user.IsActive)
        {
            return Error(new ValidationError("Аккаунт деактивирован"));
        }
        
        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Error(new ValidationError("Неверный email или пароль"));
        }
        
        var token = _jwtService.GenerateToken(user);

        var response = new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.ToString()
        };

        return Success(response);
    }
}
