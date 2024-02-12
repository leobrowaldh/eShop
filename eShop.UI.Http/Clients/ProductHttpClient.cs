
using eShop.API.DTO;
using System.Text.Json;

namespace eShop.UI.Http.Clients;

public class ProductHttpClient
{
	private readonly HttpClient _httpClient;

	public ProductHttpClient(HttpClient httpClient)
    {
		string _baseAddress = "https://localhost:7093/api/";
		_httpClient = httpClient;
		_httpClient.BaseAddress = new Uri($"{_baseAddress}products");
	}

	public async Task<List<ProductGetDTO>> GetProductsAsync(int categoryId)
	{
		try
		{
			// Use the relative path, not the base address here
			string relativePath = $"productsbycategory/{categoryId}";
			using HttpResponseMessage response = await _httpClient.GetAsync(relativePath);
			response.EnsureSuccessStatusCode();

			var resultStream = await response.Content.ReadAsStreamAsync();
			var result = await JsonSerializer.DeserializeAsync<List<ProductGetDTO>>(resultStream,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			return result ?? [];
		}
		catch (Exception ex)
		{
			return [];
		}
	}
}
