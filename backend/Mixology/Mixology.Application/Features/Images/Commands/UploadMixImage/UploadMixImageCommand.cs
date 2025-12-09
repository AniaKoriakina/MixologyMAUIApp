using Microsoft.AspNetCore.Http;
using Mixology.Application.Cqs;

namespace Mixology.Application.Features.Images.Commands.UploadMixImage;

public class UploadMixImageCommand : Command
{
    public long MixId { get; set; }
    public IFormFile Image { get; set; } = null!;
}
