
using AutoMapper;
using eShop.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace eShop.Data.Services;

public class DbService : IDbService
{
    private readonly EShopContext _db;
    private readonly IMapper _mapper;

    public DbService(EShopContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    public virtual async Task<List<TDto>> GetAsync<TEntity, TDto>() where TEntity : class where TDto : class
    {
       // IncludeNavigationsFor<TEntity>();
        var entities = await _db.Set<TEntity>().ToListAsync(); //.Set is generic, to use with any entity
        //it will only get the entities from this tables if we dont use include and add the navigation props
        return _mapper.Map<List<TDto>>(entities); //from a list of entities to a list of DTO's
    }
    public virtual async Task<TDto> SingleAsync<TEntity, TDto>(int id) where TEntity : class, IEntity where TDto : class
    {
        var entity = await _db.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == id);
        return _mapper.Map<TDto>(entity);
    }
    //Return true if Saved successfull
    public async Task<bool> SaveChangesAsync() => await _db.SaveChangesAsync() >= 0;
    public async Task<bool> DeleteAsync<TEntity>(int id) where TEntity : class, IEntity
    {
        try
        {
            var entity = await _db.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == id);
            if (entity is null) return false;
            _db.Remove(entity);
        }
        catch { return false; }

        return true;
    }

    public bool Delete<TEntity, TDto>(TDto dto) where TEntity : class where TDto : class
    {
        try
        {
            var entity = _mapper.Map<TEntity>(dto);
            if (entity is null) return false;
            _db.Remove(entity); //Remove is not async, so we do a normal method.
        }
        catch { return false; }

        return true;
    }
    public async Task<TEntity> AddAsync<TEntity, TDto>(TDto dto) where TEntity : class where TDto : class
    {
        var entity = _mapper.Map<TEntity>(dto);
        await _db.Set<TEntity>().AddAsync(entity);
        return entity;
    }
    public void IncludeNavigationsFor<TEntity>() where TEntity : class
    {
        //Looks complicated because of generics, you have to get the name of the nav props.
        // Skip Navigation Properties are used for many-to-many
        // relationsips (List or ICollection) and Navigation Properties, to skip the connectionable.
        // are used for one-to-many relationsips.
        var propertyNames = _db.Model.FindEntityType(/*reflection:*/typeof(TEntity))?.GetNavigations().Select(e => e.Name);
        var navigationPropertyNames = _db.Model.FindEntityType(typeof(TEntity))?.GetSkipNavigations().Select(e => e.Name);

        if (propertyNames is not null)
            foreach (var name in propertyNames)
                _db.Set<TEntity>().Include(name).Load();

        if (navigationPropertyNames is not null)
            foreach (var name in navigationPropertyNames)
                _db.Set<TEntity>().Include(name).Load();
    }
}
