
namespace eShop.API.DTO;

public class CategoryPostDTO
{
    public string Name { get; set; } = string.Empty;
}
public class CategoryPutDTO : CategoryPostDTO
{
    public int Id { get; set; }
}
public class CategoryGetDTO : CategoryPutDTO
{
    //public List<FilterGetDTO>? Filters { get; set; }
    public List<ProductGetDTO>? Products { get; set; }
}

//If we want to get the category objects but without the filtering, we use the following:
public class CategorySmallGetDTO : CategoryPutDTO
{
}
