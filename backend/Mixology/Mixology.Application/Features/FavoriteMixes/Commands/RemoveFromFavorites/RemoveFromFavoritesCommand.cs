using Mixology.Application.Cqs;

namespace Mixology.Application.Features.FavoriteMixes.Commands.RemoveFromFavorites;

public class RemoveFromFavoritesCommand : Command
{
    public long UserId { get; set; }
    public long MixId { get; set; }
}
