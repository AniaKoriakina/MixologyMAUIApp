using Mixology.Application.Cqs;
using Mixology.Application.Features.Brands.Queries.Dto;
using Mixology.Core.Base.Infrastructure;
using Mixology.Core.Entities;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Features.Brands.Queries.GetAllBrands;

public class GetAllBrandsQueryHandler : QueryHandler<GetAllBrandsQuery, List<BrandDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllBrandsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<List<BrandDto>>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Brand> brands;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            brands = await _unitOfWork.Brands.SearchByNameAsync(request.SearchTerm);
        }
        else
        {
            brands = await _unitOfWork.Brands.GetAllAsync();
        }

        var result = brands.Select(b => new BrandDto
        {
            Id = b.Id,
            Name = b.Name,
            Description = b.Description,
            LogoUrl = b.LogoData != null ? $"/api/images/brand/{b.Id}" : null,
            HasLogo = b.LogoData != null
        }).ToList();

        return Success(result);
    }
}
