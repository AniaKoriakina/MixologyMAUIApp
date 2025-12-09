using Microsoft.AspNetCore.Http;
using Mixology.Application.Cqs;

namespace Mixology.Application.Features.Images.Commands.UploadBrandLogo;

public class UploadBrandLogoCommand : Command
{
    public long BrandId { get; set; }
    public IFormFile Image { get; set; }
}
