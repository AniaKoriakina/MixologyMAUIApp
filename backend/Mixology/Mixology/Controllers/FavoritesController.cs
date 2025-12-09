using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mixology.Application.Features.FavoriteMixes.Commands.AddToFavorites;
using Mixology.Application.Features.FavoriteMixes.Commands.RemoveFromFavorites;
using Mixology.Application.Features.FavoriteMixes.Queries.GetFavoriteMixes;

namespace Mixology.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FavoritesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetFavoriteMixes(long userId)
    {
        var result = await _mediator.Send(new GetFavoriteMixesQuery { UserId = userId });
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddToFavorites([FromBody] AddToFavoritesCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> RemoveFromFavorites([FromBody] RemoveFromFavoritesCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
