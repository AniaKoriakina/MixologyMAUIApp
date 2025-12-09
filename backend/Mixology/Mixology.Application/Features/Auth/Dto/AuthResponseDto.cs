namespace Mixology.Application.Features.Auth.Dto;

public class AuthResponseDto
{
    public string? Token { get; set; } 
    public long UserId { get; set; }
    public string? Username { get; set; } 
    public string? Email { get; set; } 
    public string? Role { get; set; }
}
