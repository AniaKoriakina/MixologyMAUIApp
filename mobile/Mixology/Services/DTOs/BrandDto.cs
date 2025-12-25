using System.Reactive.Linq;
using ReactiveUI;

namespace Mixology.Services.DTOs;

public class BrandDto : ReactiveObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public bool HasLogo { get; set; }

    public string? FullLogoUrl
    {
        get
        {
            if (string.IsNullOrEmpty(LogoUrl)) return null;

            return LogoUrl;
        }
    }

    private ImageSource? _cachedImage;

    public ImageSource? CachedImage
    {
        get => _cachedImage;
        set => this.RaiseAndSetIfChanged(ref _cachedImage, value);
    }

    private bool _isImageLoading;

    public bool IsImageLoading
    {
        get => _isImageLoading;
        set => this.RaiseAndSetIfChanged(ref _isImageLoading, value);
    }

    private readonly ObservableAsPropertyHelper<bool> _showPlaceholder;
    public bool ShowPlaceholder => _showPlaceholder.Value;

    public BrandDto()
    {
        _showPlaceholder = this.WhenAnyValue(
                x => x.HasLogo,
                x => x.IsImageLoading,
                x => x.CachedImage,
                (hasLogo, isLoading, cachedImage) => !hasLogo || (!isLoading && cachedImage == null))
            .ToProperty(this, x => x.ShowPlaceholder);
    }
}