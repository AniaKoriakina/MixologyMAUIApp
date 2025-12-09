using Mixology.Application.Cqs;
using Mixology.Application.Services;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.Images.Commands.UploadMixImage;

public class UploadMixImageCommandHandler : CommandHandler<UploadMixImageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;

    public UploadMixImageCommandHandler(IUnitOfWork unitOfWork, IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
    }

    public override async Task<Result> Handle(UploadMixImageCommand request, CancellationToken cancellationToken)
    {
        var mix = await _unitOfWork.Mixes.GetByIdAsync(request.MixId);
        if (mix == null)
        {
            return Error(new GeneralError("Mix not found"));
        }

        var imageResult = await _imageService.ProcessImageAsync(request.Image);
        if (imageResult == null)
        {
            return Error(new ValidationError("Недопустимый формат изображения. Разрешены: JPG, PNG, GIF, WEBP. Максимальный размер: 5MB"));
        }

        mix.ImageData = imageResult.Value.Data;
        mix.ImageContentType = imageResult.Value.ContentType;
        mix.ImageFileName = imageResult.Value.FileName;

        await _unitOfWork.Mixes.UpdateAsync(mix);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success();
    }
}
