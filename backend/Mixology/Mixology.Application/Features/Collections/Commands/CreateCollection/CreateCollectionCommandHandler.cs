using Mixology.Application.Cqs;
using Mixology.Application.Services;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Entities;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.Collections.Commands.CreateCollection;

public class CreateCollectionCommandHandler : CommandHandler<CreateCollectionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;

    public CreateCollectionCommandHandler(IUnitOfWork unitOfWork, IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
    }

    public override async Task<Result> Handle(CreateCollectionCommand request, CancellationToken cancellationToken)
    {
        var collection = new Collection
        {
            Name = request.Name,
            UserId = request.UserId,
            IsDefault = false,
            Mixes = new List<Mix>()
        };

        if (request.Image != null)
        {
            var imageResult = await _imageService.ProcessImageAsync(request.Image);
            if (imageResult != null)
            {
                collection.ImageData = imageResult.Value.Data;
                collection.ImageContentType = imageResult.Value.ContentType;
                collection.ImageFileName = imageResult.Value.FileName;
            }
        }

        await _unitOfWork.Collections.AddAsync(collection);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success();
    }
}
