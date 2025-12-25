using Mixology.Services.DTOs;

namespace Mixology.Services.Requests;

public class CreateMixRequest
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    public long UserId { get; set; }
    public long CollectionId { get; set; }

    public FlavorProfile Flavor { get; set; }
    public List<CompositionDto> Compositions { get; set; } = new();
}

public record FlavorProfile(List<string> Tags);
