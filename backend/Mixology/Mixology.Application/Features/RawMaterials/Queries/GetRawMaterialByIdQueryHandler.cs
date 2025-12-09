using Mixology.Application.Cqs;
using Mixology.Application.Features.RawMaterials.Queries.Dto;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.RawMaterials.Queries;

public class GetRawMaterialByIdQueryHandler : QueryHandler<GetRawMaterialByIdQuery, RawMaterialDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRawMaterialByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<RawMaterialDto>> Handle(GetRawMaterialByIdQuery request,
        CancellationToken cancellationToken)
    {
        var rawMaterial = await _unitOfWork.RawMaterials.GetByIdAsync(request.Id);
        if (rawMaterial == null) 
            return Error(new GeneralError("Unknown Raw Material"));

        return Success(new RawMaterialDto
        {
            Id = rawMaterial.Id,
            Name = rawMaterial.Name,
            Flavor = rawMaterial.Flavor,
            Strength = rawMaterial.Strength,
            BrandId = rawMaterial.BrandId
        });
    }
}