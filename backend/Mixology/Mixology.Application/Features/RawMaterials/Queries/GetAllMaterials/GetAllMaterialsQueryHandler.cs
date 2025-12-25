using Microsoft.Extensions.Logging;
using Mixology.Application.Cqs;
using Mixology.Application.Features.RawMaterials.Queries.Dto;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Entities;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.RawMaterials.Queries.GetAllMaterials;

public class GetAllMaterialsQueryHandler : QueryHandler<GetAllMaterialsQuery, List<RawMaterialDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllMaterialsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<List<RawMaterialDto>>> Handle(GetAllMaterialsQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<RawMaterial> materials;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            materials = await _unitOfWork.RawMaterials.SearchByNameOrFlavorAsync(request.SearchTerm);
        }
        else
        {
            materials = await _unitOfWork.RawMaterials.GetAllAsync();
        }

        var result = materials.Select(m => new RawMaterialDto
        {
            Id = m.Id,
            Name = m.Name,
            BrandId = m.BrandId,
            Flavor = m.Flavor,
            Strength = m.Strength,
        }).ToList();
        
        return Success(result);
    }
}