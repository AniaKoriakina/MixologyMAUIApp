using Microsoft.AspNetCore.Http;

namespace Mixology.Application.Services;

public interface IImageService
{
    Task<(byte[] Data, string ContentType, string FileName)?> ProcessImageAsync(IFormFile file, int maxWidth = 800, int quality = 85);
    bool ValidateImage(IFormFile file);
    string GetBase64Image(byte[] imageData, string contentType);
}
