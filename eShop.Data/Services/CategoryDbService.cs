using AutoMapper;
using eShop.Data.Contexts;

namespace eShop.Data.Services;

public class CategoryDbService(EShopContext db, IMapper mapper) : DbService(db, mapper)
{
}
