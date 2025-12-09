using Microsoft.AspNetCore.Http;
using Mixology.Application.Cqs;

namespace Mixology.Application.Features.Images.Commands.UploadCollectionImage;

public class UploadCollectionImageCommand : Command
{
    public long CollectionId { get; set; }
    public IFormFile Image { get; set; } = null!;
}
