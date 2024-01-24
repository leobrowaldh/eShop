namespace eShop.API.DTO;

public class ProductPostDTO
{
    public string Name { get; set; } = string.Empty;
}
public class ProductPutDTO : ProductPostDTO
{
    public int Id { get; set; }
}
public class ProductGetDTO : ProductPutDTO
{
    //public List<FilterGetDTO>? Filters { get; set; }
}