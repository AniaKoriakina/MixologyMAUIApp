using System.Net.Http.Json;
using Mixology.Services.Requests;

namespace Mixology.Services;

public class NotificationService
{
    private readonly HttpClient _httpClient;

    public NotificationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> SendDeviceTokenAsync(RecipientRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Recipient", request);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}