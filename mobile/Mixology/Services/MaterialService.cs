using System.Net.Http.Json;
using System.Text.Json;
using Mixology.Services.DTOs;
using Mixology.Services.Responses;

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
}
