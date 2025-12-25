using System.Net.Http.Json;
using Mixology.Services.DTOs;
using Mixology.Services.Responses.Base;

namespace Mixology.Services;

public class BrandService
{
    private readonly HttpClient _httpClient;

    public BrandService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<List<BrandDto>> GetBrandsAsync(string searchTerm = "")
    {
        var url = $"/api/Brands?searchTerm={Uri.EscapeDataString(searchTerm)}";
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<BrandDto>>>(url);
        return response?.Value ?? new List<BrandDto>();
    }
}