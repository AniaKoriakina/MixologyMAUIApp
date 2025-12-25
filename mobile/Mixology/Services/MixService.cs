using System.Net.Http.Json;
using System.Text.Json;
using Mixology.Services.DTOs;
using Mixology.Services.Requests;
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

    public async Task<PagedResult<MixDto>> GetMixesPagedAsync(string? searchText, string? orderBy, int page = 1,
        int pageSize = 5)
    {
        try
        {
            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(searchText))
                queryParams.Add($"searchTerm={Uri.EscapeDataString(searchText)}");
            if (!string.IsNullOrWhiteSpace(orderBy))
                queryParams.Add($"sortBy={Uri.EscapeDataString(orderBy)}");

            queryParams.Add($"page={page}");
            queryParams.Add($"pageSize={pageSize}");

            var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";

            var response = await _httpClient.GetFromJsonAsync<PagedMixServiceResponse>(
                $"api/Mixes/search{query}",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return response?.Value ?? new PagedResult<MixDto>();
        }
        catch (Exception ex)
        {
            return new PagedResult<MixDto>();
        }
    }

    public async Task<bool> CreateMixAsync(CreateMixRequest mix)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Mixes", mix);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}