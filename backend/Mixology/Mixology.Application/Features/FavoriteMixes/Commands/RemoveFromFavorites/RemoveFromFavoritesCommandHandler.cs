using Mixology.Application.Cqs;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.FavoriteMixes.Commands.RemoveFromFavorites;

public class RemoveFromFavoritesCommandHandler : CommandHandler<RemoveFromFavoritesCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveFromFavoritesCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result> Handle(RemoveFromFavoritesCommand request, CancellationToken cancellationToken)
    {
        var isFavorite = await _unitOfWork.FavoriteMixes.IsFavoriteAsync(request.UserId, request.MixId);
        if (!isFavorite)
        {
            return Error(new ValidationError("Mix not in favorites"));
        }

        await _unitOfWork.FavoriteMixes.RemoveFavoriteAsync(request.UserId, request.MixId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success();
    }
}
