
using Mixology.Services.Requests;
using Mixology.Views.Pages;
using System.Net.Http;
using System.Net.Http.Json;

namespace Mixology.Services;

public class UserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> Register(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", request);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}