using Mixology.Application.Cqs;
using Mixology.Application.Services;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.Images.Commands.UploadCollectionImage;

public class UploadCollectionImageCommandHandler : CommandHandler<UploadCollectionImageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;

    public UploadCollectionImageCommandHandler(IUnitOfWork unitOfWork, IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
    }

    public override async Task<Result> Handle(UploadCollectionImageCommand request, CancellationToken cancellationToken)
    {
        var collection = await _unitOfWork.Collections.GetByIdAsync(request.CollectionId);
        if (collection == null)
        {
            return Error(new GeneralError("Collection not found"));
        }

        var imageResult = await _imageService.ProcessImageAsync(request.Image);
        if (imageResult == null)
        {
            return Error(new ValidationError("Недопустимый формат изображения. Разрешены: JPG, PNG, GIF, WEBP. Максимальный размер: 5MB"));
        }

        collection.ImageData = imageResult.Value.Data;
        collection.ImageContentType = imageResult.Value.ContentType;
        collection.ImageFileName = imageResult.Value.FileName;

        await _unitOfWork.Collections.UpdateAsync(collection);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success();
    }
}
