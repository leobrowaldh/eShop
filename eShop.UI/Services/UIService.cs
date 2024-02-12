


namespace eShop.UI.Services;

public class UIService(CategoryHttpClient categoryHttpClient, ProductHttpClient productHttpClient, IMapper mapper )
{
	List<CategoryGetDTO> Categories { get; set; } = [];
	public List<ProductGetDTO> Products { get; private set; } = [];
	public List<LinkGroup> CaregoryLinkGroups { get; private set; } =
	[
		new LinkGroup 
		{ 
			Name = "Categories"
			//,
			//LinkOptions = new List<LinkOption>()
			//{	//test data:
			// 	new LinkOption {Id = 1, Name = "Woman", IsSelected = true},
			// 	new LinkOption {Id = 2, Name = "Men", IsSelected = false},
			// 	new LinkOption {Id = 3, Name = "Children", IsSelected = false}
			//}
		}
	];
	public int CurrentCategoryId { get; set; }

	public async Task GetLinkGroup()
	{
		var categories = await categoryHttpClient.GetCategoriesAsync();
		CaregoryLinkGroups[0].LinkOptions = mapper.Map<List<LinkOption>>(Categories);
		var linkOption = CaregoryLinkGroups[0].LinkOptions.FirstOrDefault();
		//when asigning a value we also need to use ! instead of ?
		linkOption !.IsSelected = true;
	}

	public async Task OnCategoryLinkClick(int id)
	{
		CurrentCategoryId = id;
		await GetProductsAsync();
		//now for the symbol that shows selected category on the aside component:
		CaregoryLinkGroups[0].LinkOptions.ForEach(l => l.IsSelected = false);
		CaregoryLinkGroups[0].LinkOptions.Single(l => l.Id.Equals(CurrentCategoryId)).IsSelected = true;
	}

	public async Task GetProductsAsync() => Products = await productHttpClient.GetProductsAsync(CurrentCategoryId);
}
