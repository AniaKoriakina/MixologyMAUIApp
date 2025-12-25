using System.Collections.Concurrent;

namespace Mixology.Services;

public class ImageCacheService
{
    private readonly HttpClient _httpClient;
    private readonly ConcurrentDictionary<string, byte[]> _imageCache = new();
    private readonly ConcurrentDictionary<string, bool> _failedUrls = new();

    public ImageCacheService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ImageSource?> GetImageAsync(string? url)
    {
        if (string.IsNullOrEmpty(url) || _failedUrls.ContainsKey(url))
            return null;

        try
        {
            if (_imageCache.TryGetValue(url, out var cachedImage))
            {
                return ImageSource.FromStream(() => new MemoryStream(cachedImage));
            }

            using var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _failedUrls.TryAdd(url, true);
                return null;
            }

            var contentType = response.Content.Headers.ContentType?.MediaType;

            if (!IsImageContentType(contentType))
            {
                _failedUrls.TryAdd(url, true);
                return null;
            }

            var imageBytes = await response.Content.ReadAsByteArrayAsync();
            if (imageBytes == null || imageBytes.Length == 0)
            {
                _failedUrls.TryAdd(url, true);
                return null;
            }

            _imageCache.TryAdd(url, imageBytes);

            return ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка загрузки изображения {url}: {ex.Message}");
            _failedUrls.TryAdd(url, true);
            return null;
        }
    }

    private static bool IsImageContentType(string? contentType)
    {
        if (string.IsNullOrEmpty(contentType))
            return false;

        return contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
    }

    public void ClearCache()
    {
        _imageCache.Clear();
        _failedUrls.Clear();
    }

    public void RetryFailedUrl(string url)
    {
        _failedUrls.TryRemove(url, out _);
    }

    public async Task<ImageSource?> RetryImageAsync(string? url)
    {
        if (string.IsNullOrEmpty(url)) return null;

        _failedUrls.TryRemove(url, out _);
        _imageCache.TryRemove(url, out _);

        return await GetImageAsync(url);
    }
}