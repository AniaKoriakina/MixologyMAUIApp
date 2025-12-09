using Microsoft.AspNetCore.Http;
using Mixology.Application.Cqs;

namespace Mixology.Application.Features.Collections.Commands.CreateCollection;

public class CreateCollectionCommand : Command
{
    public string Name { get; set; }
    public IFormFile? Image { get; set; }
    public long UserId { get; set; }
}
