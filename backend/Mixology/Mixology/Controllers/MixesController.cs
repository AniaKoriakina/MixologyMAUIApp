using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mixology.Application.Features.Mixes.Commands.CreateMix;
using Mixology.Application.Features.Mixes.Queries.GetMixById;
using Mixology.Application.Features.Mixes.Queries.GetUserMixes;
using Mixology.Application.Features.Mixes.Queries.SearchMixes;

namespace Mixology.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MixesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MixesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> CreateMix([FromBody] CreateMixCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMixById(long id)
    {
        var result = await _mediator.Send(new GetMixByIdQuery { MixId = id });
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchMixes(
        [FromQuery] string? searchTerm, 
        [FromQuery] string? sortBy,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new SearchMixesQuery 
        { 
            SearchTerm = searchTerm, 
            SortBy = sortBy,
            Page = page,
            PageSize = pageSize
        });
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserMixes(
        long userId, 
        [FromQuery] string? searchTerm,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetUserMixesQuery 
        { 
            UserId = userId, 
            SearchTerm = searchTerm,
            Page = page,
            PageSize = pageSize
        });
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
