using Mixology.Application.Cqs;
using Mixology.Application.Features.Brands.Queries.Dto;
using Mixology.Application.Features.RawMaterials.Queries.Dto;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.Brands.Queries.GetBrandWithMaterials;

public class GetBrandWithMaterialsQueryHandler : QueryHandler<GetBrandWithMaterialsQuery, BrandWithMaterialsDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBrandWithMaterialsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<BrandWithMaterialsDto>> Handle(GetBrandWithMaterialsQuery request, CancellationToken cancellationToken)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(request.BrandId);
        if (brand == null)
        {
            return Error(new GeneralError("Brand not found"));
        }

        var materials = await _unitOfWork.RawMaterials.GetByBrandIdAsync(request.BrandId);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            materials = materials.Where(m => 
                m.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                m.Flavor.Tags.Any(t => t.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
            );
        }

        var dto = new BrandWithMaterialsDto
        {
            Id = brand.Id,
            Name = brand.Name,
            Description = brand.Description,
            LogoUrl = brand.LogoData != null ? $"/api/images/brand/{brand.Id}" : null,
            HasLogo = brand.LogoData != null,
            RawMaterials = materials.Select(m => new RawMaterialDto
            {
                Id = m.Id,
                Name = m.Name,
                Flavor = m.Flavor,
                Strength = m.Strength,
                BrandId = m.BrandId
            }).ToList()
        };

        return Success(dto);
    }
}
