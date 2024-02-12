
using eShop.API.DTO;
using System.Text.Json;

namespace eShop.UI.Http.Clients;

public class CategoryHttpClient
{
	private readonly HttpClient _httpClient;

	public CategoryHttpClient(HttpClient httpClient)
    {
		string _baseAddress = "https://localhost:5000/api/";
		_httpClient = httpClient;
		_httpClient.BaseAddress = new Uri($"{_baseAddress}categorys");
	}

	public async Task<List<CategoryGetDTO>> GetCategoriesAsync()
	{
		try
		{
			using HttpResponseMessage response = await _httpClient.GetAsync(""); //inbuilt method
			response.EnsureSuccessStatusCode(); //throws ex if not 200 response
												//now we need to serialize the incomming json
			var result = JsonSerializer.Deserialize<List<CategoryGetDTO>>(await response.Content.ReadAsStreamAsync(),
				//case sensitive part is because Json serializes to lowercamel case for prop names
					new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			return result ?? [];
		}
		catch (Exception ex) 
		{
			return [];
		}
	}
}
