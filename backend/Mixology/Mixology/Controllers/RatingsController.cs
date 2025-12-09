using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mixology.Application.Features.Ratings.Commands.RateMix;

namespace Mixology.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RatingsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> RateMix([FromBody] RateMixCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
