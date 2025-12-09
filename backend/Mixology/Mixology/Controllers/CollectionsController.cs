using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mixology.Application.Features.Collections.Commands.CreateCollection;
using Mixology.Application.Features.Collections.Queries.GetUserCollections;

namespace Mixology.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollectionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CollectionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserCollections(long userId)
    {
        var result = await _mediator.Send(new GetUserCollectionsQuery { UserId = userId });
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateCollection([FromForm] CreateCollectionCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
