
namespace eShop.API.Extensions.Extensions;

//Extension methods to use on our API app instance, so we dont need to repeat this code over and over.
public static class HttpExtensions
{
    public static void AddEndpoint<TEntity, TPostDto, TPutDto, TGetDto>(this WebApplication app) //we dont need Delete, since it only need the Id
    where TEntity : class, IEntity where TPostDto : class where TPutDto : class where TGetDto : class
    {
        var node = typeof(TEntity).Name.ToLower();
        //app.MapGet($"/api/{node}s/" + "{id}", HttpSingleAsync<TEntity, TGetDto>);
        app.MapGet($"/api/{node}s", HttpGetAsync<TEntity, TGetDto>);
        //app.MapPost($"/api/{node}s", HttpPostAsync<TEntity, TPostDto>);
        //app.MapPut($"/api/{node}s/" + "{id}", HttpPutAsync<TEntity, TPutDto>);
        //app.MapDelete($"/api/{node}s/" + "{id}", HttpDeleteAsync<TEntity>);
    }

    public static async Task<IResult> HttpGetAsync<TEntity, TDto>(this IDbService db)
    where TEntity : class where TDto : class =>
       Results.Ok(await db.GetAsync<TEntity, TDto>());
}
