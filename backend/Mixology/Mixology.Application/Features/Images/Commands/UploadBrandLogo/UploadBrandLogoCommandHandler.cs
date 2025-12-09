using Mixology.Application.Cqs;
using Mixology.Application.Services;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.Images.Commands.UploadBrandLogo;

public class UploadBrandLogoCommandHandler : CommandHandler<UploadBrandLogoCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;

    public UploadBrandLogoCommandHandler(IUnitOfWork unitOfWork, IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
    }

    public override async Task<Result> Handle(UploadBrandLogoCommand request, CancellationToken cancellationToken)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(request.BrandId);
        if (brand == null)
        {
            return Error(new GeneralError("Brand not found"));
        }

        var imageResult = await _imageService.ProcessImageAsync(request.Image);
        if (imageResult == null)
        {
            return Error(new ValidationError("Недопустимый формат изображения. Разрешены: JPG, PNG, GIF, WEBP. Максимальный размер: 5MB"));
        }

        brand.LogoData = imageResult.Value.Data;
        brand.LogoContentType = imageResult.Value.ContentType;
        brand.LogoFileName = imageResult.Value.FileName;

        await _unitOfWork.Brands.UpdateAsync(brand);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success();
    }
}
