using System.Net.Http.Json;
using System.Text.Json;
using Mixology.Services.DTOs;
using Mixology.Services.Responses;

namespace Mixology.Services;

public class MixService
{
    private readonly HttpClient _httpClient;

    public MixService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    public async Task<List<MixDto>> GetMixesAsync()
    {
        return await GetMixesAsync(null, null);
    }
    
    public async Task<List<MixDto>> GetMixesAsync(string? searchText, string? orderBy)
    {
        try
        {
            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(searchText))
                queryParams.Add($"searchTerm={Uri.EscapeDataString(searchText)}");
            if (!string.IsNullOrWhiteSpace(orderBy))
                queryParams.Add($"sortBy={Uri.EscapeDataString(orderBy)}");
            
            var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            
            var response = await _httpClient.GetFromJsonAsync<MixServiceResponse>(
                $"api/Mixes/search{query}",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return response?.Value ?? new List<MixDto>();
        }
        catch(Exception ex)
        {
            return new List<MixDto>();
        }
    }

}