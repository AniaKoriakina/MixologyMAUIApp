using Microsoft.AspNetCore.Http;

namespace Mixology.Application.Services;

public class ImageService : IImageService
{
    private const long MaxFileSize = 5 * 1024 * 1024; 
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private static readonly string[] AllowedContentTypes = 
    { 
        "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" 
    };
    
    private static readonly byte[][] ImageSignatures = 
    {
        new byte[] { 0xFF, 0xD8, 0xFF }, // JPEG
        new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, // PNG
        new byte[] { 0x47, 0x49, 0x46, 0x38 }, // GIF
        new byte[] { 0x52, 0x49, 0x46, 0x46 }, // WEBP
    };

    public bool ValidateImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        if (file.Length > MaxFileSize)
            return false;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return false;

        if (!AllowedContentTypes.Contains(file.ContentType.ToLowerInvariant()))
            return false;
        
        try
        {
            using var reader = new BinaryReader(file.OpenReadStream());
            var headerBytes = reader.ReadBytes(8);
            
            return ImageSignatures.Any(signature => 
                headerBytes.Take(signature.Length).SequenceEqual(signature));
        }
        catch
        {
            return false;
        }
    }

    public async Task<(byte[] Data, string ContentType, string FileName)?> ProcessImageAsync(
        IFormFile file, int maxWidth = 800, int quality = 85)
    {
        if (!ValidateImage(file))
            return null;

        try
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var imageData = memoryStream.ToArray();
            return (imageData, file.ContentType, file.FileName);
        }
        catch
        {
            return null;
        }
    }

    public string GetBase64Image(byte[] imageData, string contentType)
    {
        if (imageData == null || imageData.Length == 0)
            return string.Empty;

        var base64 = Convert.ToBase64String(imageData);
        return $"data:{contentType};base64,{base64}";
    }
}
