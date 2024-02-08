

namespace eShop.UI.Services;

public class UIService(CategoryHttpClient categoryHttpClient)
{
	List<CategoryGetDTO> Categories { get; set; } = [];
	public List<LinkGroup> CaregoryLinkGroups { get; private set; } =
	[
		new LinkGroup 
		{ 
			Name = "Categories",
			LinkOptions = new List<LinkOption>()
			{	//test data:
			 	new LinkOption {Id = 1, Name = "Woman", IsSelected = true},
			 	new LinkOption {Id = 2, Name = "Men", IsSelected = false},
			 	new LinkOption {Id = 3, Name = "Children", IsSelected = false}
			}
		}
	];
	public int CurrentCategoryId { get; set; }
}
