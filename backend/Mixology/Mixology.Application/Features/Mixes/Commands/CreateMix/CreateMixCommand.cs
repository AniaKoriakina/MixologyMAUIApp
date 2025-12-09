using Microsoft.AspNetCore.Http;
using Mixology.Application.Cqs;
using Mixology.Core.ValueObjects;

namespace Mixology.Application.Features.Mixes.Commands.CreateMix;

public class CreateMixCommand : Command
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
    public long UserId { get; set; }
    public long? CollectionId { get; set; }
    public FlavorProfile Flavor { get; set; }
    public List<MixComposition> Compositions { get; set; } = [];
}
