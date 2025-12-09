using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mixology.Application.Features.Auth.Commands.Login;
using Mixology.Application.Features.Auth.Commands.Register;
using Mixology.Application.Features.Auth.Queries.GetCurrentUser;

namespace Mixology.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess 
            ? Ok(new { message = "Регистрация успешна" }) 
            : BadRequest(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                         ?? User.FindFirst("sub")?.Value;
        
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Неверный токен" });
        }

        var result = await _mediator.Send(new GetCurrentUserQuery { UserId = userId });
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
