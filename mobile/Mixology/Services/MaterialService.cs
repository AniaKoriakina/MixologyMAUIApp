using System.Net.Http.Json;
using System.Text.Json;
using Mixology.Services.DTOs;
using Mixology.Services.Responses;
using Mixology.Services.Responses.Base;

namespace Mixology.Services;

public class MaterialService
{
    private readonly HttpClient _httpClient;

    public MaterialService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RawMaterialDto> GetRawMaterial(string materialId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<MaterialServiceResponse>(
                $"api/RawMaterials/{materialId}",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return response?.Value ?? new RawMaterialDto();
        }
        catch (Exception ex)
        {
            return new RawMaterialDto();
        }
    }
    
    public async Task<List<RawMaterialDto>> GetMaterials(string searchTerm = "")
    {
        var url = $"/api/RawMaterials?searchTerm={Uri.EscapeDataString(searchTerm)}";
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<RawMaterialDto>>>(url);

        if (response == null || !response.IsSuccess)
            return new List<RawMaterialDto>();

        return response.Value ?? new List<RawMaterialDto>();
    }
}
