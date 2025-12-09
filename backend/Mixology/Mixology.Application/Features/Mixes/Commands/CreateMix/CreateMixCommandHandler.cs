using Mixology.Application.Cqs;
using Mixology.Application.Services;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Entities;
using Mixology.Core.Shared.Result;
using Mixology.Core.ValueObjects;

namespace Mixology.Application.Features.Mixes.Commands.CreateMix;

public class CreateMixCommandHandler : CommandHandler<CreateMixCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;

    public CreateMixCommandHandler(IUnitOfWork unitOfWork, IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
    }

    public override async Task<Result> Handle(CreateMixCommand request, CancellationToken cancellationToken)
    {
        var totalPercentage = request.Compositions.Sum(c => c.Percentage);
        if (Math.Abs(totalPercentage - 100) > 0.01)
        {
            return Error(new ValidationError("Сумма процентов должна быть равна 100"));
        }
        
        var collectionId = request.CollectionId;
        if (!collectionId.HasValue)
        {
            var defaultCollection = await _unitOfWork.Collections.GetDefaultCollectionAsync(request.UserId);
            if (defaultCollection == null)
            {
                defaultCollection = new Collection
                {
                    Name = "Моя коллекция",
                    UserId = request.UserId,
                    IsDefault = true,
                    Mixes = new List<Mix>()
                };
                await _unitOfWork.Collections.AddAsync(defaultCollection);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            collectionId = defaultCollection.Id;
        }
        
        var name = request.Name;
        if (string.IsNullOrWhiteSpace(name))
        {
            var userMixes = await _unitOfWork.Mixes.GetByUserIdAsync(request.UserId);
            var count = userMixes.Count() + 1;
            name = $"Классный микс №{count}";
        }

        var mix = new Mix
        {
            Name = name,
            Description = request.Description ?? GenerateDescription(request.Flavor),
            UserId = request.UserId,
            CollectionId = collectionId,
            Flavor = request.Flavor,
            Compositions = request.Compositions,
            RatingAverage = 0
        };
        
        if (request.Image != null)
        {
            var imageResult = await _imageService.ProcessImageAsync(request.Image);
            if (imageResult != null)
            {
                mix.ImageData = imageResult.Value.Data;
                mix.ImageContentType = imageResult.Value.ContentType;
                mix.ImageFileName = imageResult.Value.FileName;
            }
        }

        await _unitOfWork.Mixes.AddAsync(mix);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success();
    }

    private string GenerateDescription(FlavorProfile flavor)
    {
        return $"Микс со вкусами: {string.Join(", ", flavor.Tags)}";
    }
}
