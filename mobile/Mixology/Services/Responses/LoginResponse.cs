namespace Mixology.Services.Responses;

public class LoginResponse
{
    public string Token { get; set; }
    public long UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}