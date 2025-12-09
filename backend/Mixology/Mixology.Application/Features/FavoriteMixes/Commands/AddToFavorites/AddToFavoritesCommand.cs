using Mixology.Application.Cqs;

namespace Mixology.Application.Features.FavoriteMixes.Commands.AddToFavorites;

public class AddToFavoritesCommand : Command
{
    public long UserId { get; set; }
    public long MixId { get; set; }
}
