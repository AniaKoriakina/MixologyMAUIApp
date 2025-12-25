
using Mixology.Services.Requests;
using Mixology.Views.Pages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Mixology.Services.Responses;
using Mixology.Services.Responses.Base;

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

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", request);
            response.EnsureSuccessStatusCode();

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
            
            if (apiResponse != null && apiResponse.IsSuccess && apiResponse.Value != null)
            {
                var loginResponse = apiResponse.Value;

                if (!string.IsNullOrEmpty(loginResponse.Token))
                {
                    await SaveTokenAsync(loginResponse.Token);
                }

                return loginResponse;
            }
            
            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task SaveTokenAsync(string token)
    {
        try
        {
            await SecureStorage.Default.SetAsync("token", token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    public async Task<string> GetTokenAsync()
    {
        try
        {
            return await SecureStorage.Default.GetAsync("token");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<UserInfoResponse> GetCurrentUserInfo(string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var user = await _httpClient.GetFromJsonAsync<ApiResponse<UserInfoResponse>>("api/Auth/me");
            return user.Value;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task LogoutAsync()
    {
        try
        {
            await SecureStorage.Default.SetAsync("token", string.Empty);
            SecureStorage.Default.Remove("token");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при логауте: {ex.Message}");
        }
    }
}