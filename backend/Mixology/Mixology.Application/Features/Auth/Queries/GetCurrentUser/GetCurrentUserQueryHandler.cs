using Mixology.Application.Cqs;
using Mixology.Application.Features.Auth.Dto;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : QueryHandler<GetCurrentUserQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCurrentUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user == null)
        {
            return Error(new GeneralError("Пользователь не найден"));
        }

        var dto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.ToString(),
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };

        return Success(dto);
    }
}
