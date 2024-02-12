
using eShop.API.DTO;
using eShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Data.Services;

public class ProductDbService(EShopContext db, IMapper mapper) : DbService(db, mapper)
{
    public override async Task<List<TDto>> GetAsync<TEntity, TDto>()
    {
        //IncludeNavigationsFor<Filter>();
        //IncludeNavigationsFor<Product>();
        return await base.GetAsync<TEntity, TDto>();
    }

	public async Task<List<ProductGetDTO>> GetProductsByCategoryAsync(int categoryId)
	{
		IncludeNavigationsFor<Color>();
		IncludeNavigationsFor<Size>();
		//first we get all product ids in the selected category:
		var productIds = GetAsync<ProductCategory>(pc => pc.CategoryId.Equals(categoryId))
			.Select(pc => pc.ProductId);
		//then we get the products with the selected ids:
		var products = await GetAsync<Product>(p => productIds.Contains(p.Id)).ToListAsync();
		return MapList<Product, ProductGetDTO>(products);
	}
}