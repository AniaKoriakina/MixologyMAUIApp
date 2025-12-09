using Mixology.Application.Cqs;
using Mixology.Application.Services;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Entities;
using Mixology.Core.Enum;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : CommandHandler<RegisterCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public override async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || request.Username.Length < 3)
        {
            return Error(new ValidationError("Имя пользователя должно содержать минимум 3 символа"));
        }

        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
        {
            return Error(new ValidationError("Некорректный email адрес"));
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
        {
            return Error(new ValidationError("Пароль должен содержать минимум 6 символов"));
        }

        var existingUser = await _unitOfWork.Users.GetUserByEmail(request.Email);
        if (existingUser != null)
        {
            return Error(new ValidationError("Пользователь с таким email уже существует"));
        }
        
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Role = Role.User,
            IsActive = true
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var defaultCollection = new Collection
        {
            Name = "Моя коллекция",
            UserId = user.Id,
            IsDefault = true
        };
        await _unitOfWork.Collections.AddAsync(defaultCollection);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success();
    }
}
