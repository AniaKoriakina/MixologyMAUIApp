using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mixology.Application.Features.Brands.Queries.GetAllBrands;
using Mixology.Application.Features.Brands.Queries.GetBrandWithMaterials;

namespace Mixology.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BrandsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllBrands([FromQuery] string? searchTerm)
    {
        var result = await _mediator.Send(new GetAllBrandsQuery { SearchTerm = searchTerm });
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
    [HttpGet("{brandId}")]
    public async Task<IActionResult> GetBrandWithMaterials(long brandId, [FromQuery] string? searchTerm)
    {
        var result = await _mediator.Send(new GetBrandWithMaterialsQuery 
        { 
            BrandId = brandId, 
            SearchTerm = searchTerm 
        });
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
