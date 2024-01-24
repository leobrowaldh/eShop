
using Microsoft.AspNetCore.Mvc;

namespace eShop.API.Extensions.Extensions;

//Extension methods to use on our API app instance, generics so we dont need to repeat this code over and over.
public static class HttpExtensions
{
    public static void AddEndpoint<TEntity, TPostDto, TPutDto, TGetDto>(this WebApplication app) //we dont need Delete, since it only need the Id
    where TEntity : class, IEntity where TPostDto : class where TPutDto : class where TGetDto : class
    {
        var node = typeof(TEntity).Name.ToLower();
        app.MapGet($"/api/{node}s/" + "{id}", HttpSingleAsync<TEntity, TGetDto>);
        app.MapGet($"/api/{node}s", HttpGetAsync<TEntity, TGetDto>);
        //app.MapPost($"/api/{node}s", HttpPostAsync<TEntity, TPostDto>);
        //app.MapPut($"/api/{node}s/" + "{id}", HttpPutAsync<TEntity, TPutDto>);
        app.MapDelete($"/api/{node}s/" + "{id}", HttpDeleteAsync<TEntity>);
    }
    //These are for the many to many tables only, they hade no singular Id, and no Entity, different handling required.
    //Only post and delete are needed in manytomany tables.
    public static void AddEndpoint<TEntity, TPostDto, TDeleteDto>(this WebApplication app)
    where TEntity : class where TPostDto : class where TDeleteDto : class
    {
        var node = typeof(TEntity).Name.ToLower();
        //same address, no conflict, cause we will use different API for this.
        app.MapPost($"/api/{node}s", HttpPostReferenceAsync<TEntity, TPostDto>);

        //From body means the DTO is not in the URL, but in the data sent with the request.
        app.MapDelete($"/api/{node}s", async (IDbService db, [FromBody] TDeleteDto dto) =>
        {
            try
            {
                if (!db.Delete<TEntity, TDeleteDto>(dto)) return Results.NotFound();

                if (await db.SaveChangesAsync()) return Results.NoContent();
            }
            catch
            {
            }

            return Results.BadRequest($"Couldn't delete the {typeof(TEntity).Name} entity.");
        });
    }

    public static async Task<IResult> HttpGetAsync<TEntity, TDto>(this IDbService db)
    where TEntity : class where TDto : class =>
       Results.Ok(await db.GetAsync<TEntity, TDto>());

    public static async Task<IResult> HttpSingleAsync<TEntity, TDto>(this IDbService db, int id)
   where TEntity : class, IEntity where TDto : class
    {
        var result = await db.SingleAsync<TEntity, TDto>(id);
        if (result is null) return Results.NotFound();
        return Results.Ok(result);
    }
    public static async Task<IResult> HttpDeleteAsync<TEntity>(this IDbService db, int id) //This cannot delete items in many to many connection tables, since they have composite keys, we need another delete method for this.
    where TEntity : class, IEntity
    {
        try
        {
            if (!await db.DeleteAsync<TEntity>(id)) return Results.NotFound();

            if (await db.SaveChangesAsync()) return Results.NoContent(); //we dont need to return any content to the UI
        }
        catch
        {
        }

        return Results.BadRequest($"Couldn't delete the {typeof(TEntity).Name} entity.");
    }
    public static async Task<IResult> HttpPostReferenceAsync<TEntity, TPostDto>(this IDbService db, TPostDto dto)
    where TEntity : class where TPostDto : class
    {
        try
        {
            var entity = await db.AddAsync<TEntity, TPostDto>(dto);
            if (await db.SaveChangesAsync())
            {
                var node = typeof(TEntity).Name.ToLower();
                return Results.Created($"/{node}s/", entity);
            }
        }
        catch
        {
        }

        return Results.BadRequest($"Couldn't add the {typeof(TEntity).Name} entity.");
    }
}
