using Mixology.Application.Cqs;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Entities;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.FavoriteMixes.Commands.AddToFavorites;

public class AddToFavoritesCommandHandler : CommandHandler<AddToFavoritesCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddToFavoritesCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result> Handle(AddToFavoritesCommand request, CancellationToken cancellationToken)
    {
        var mix = await _unitOfWork.Mixes.GetByIdAsync(request.MixId);
        if (mix == null)
        {
            return Error(new GeneralError("Mix not found"));
        }

        var isFavorite = await _unitOfWork.FavoriteMixes.IsFavoriteAsync(request.UserId, request.MixId);
        if (isFavorite)
        {
            return Error(new ValidationError("Mix already in favorites"));
        }

        var favorite = new FavoriteMix
        {
            UserId = request.UserId,
            MixId = request.MixId
        };

        await _unitOfWork.FavoriteMixes.AddAsync(favorite);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success();
    }
}
