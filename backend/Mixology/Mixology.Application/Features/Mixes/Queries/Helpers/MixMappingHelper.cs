using Mixology.Application.Features.Mixes.Queries.Dto;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.ValueObjects;

namespace Mixology.Application.Features.Mixes.Queries.Helpers;

public static class MixMappingHelper
{
    public static async Task<List<MixCompositionDto>> MapCompositionsAsync(
        List<MixComposition> compositions, 
        IUnitOfWork unitOfWork)
    {
        if (!compositions.Any())
            return new List<MixCompositionDto>();
        
        var tobaccoIds = compositions.Select(c => c.TobaccoId).Distinct().ToList();
        var tobaccos = await unitOfWork.RawMaterials.GetByIdsAsync(tobaccoIds);
        var tobaccoDict = tobaccos.ToDictionary(t => t.Id, t => t.Name);

        return compositions.Select(composition => new MixCompositionDto
        {
            TobaccoId = composition.TobaccoId,
            TobaccoName = tobaccoDict.GetValueOrDefault(composition.TobaccoId, "Неизвестно"),
            Percentage = composition.Percentage
        }).ToList();
    }
}
