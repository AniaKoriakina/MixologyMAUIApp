using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mixology.Application.Features.RawMaterials.Queries;
using Mixology.Application.Features.RawMaterials.Queries.GetAllMaterials;
using Mixology.Application.Features.RawMaterials.Queries.GetMaterialById;
using Mixology.Core.Base.Infrastructure;

namespace Mixology.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RawMaterialsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RawMaterialsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetMaterials([FromQuery] string? searchTerm)
    {
        var result = await _mediator.Send(new GetAllMaterialsQuery() { SearchTerm = searchTerm });
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaterialById(long id)
    {
        var result = await _mediator.Send(new GetRawMaterialByIdQuery { Id = id });
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}