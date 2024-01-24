

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// SQL Server Service Registration
builder.Services.AddDbContext<EShopContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("EShopConnection")));

/**********
 ** CORS Cross-Origin Resource Sharing**  ¨who can use our api
 **********/
builder.Services.AddCors(policy =>
{
    policy.AddPolicy("CorsAllAccessPolicy", opt =>
        opt.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod()
    );
});

RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

RegisterEndpoints();

/************************
 ** CORS Configuration **
 ************************/
app.UseCors("CorsAllAccessPolicy");

app.Run();

void RegisterEndpoints()
{
    app.AddEndpoint<Category, CategoryPostDTO, CategoryPutDTO, CategoryGetDTO>();
    //app.MapGet($"/api/categorieswithdata", async (IDbService db) =>
    //{
    //    try
    //    {
    //        return Results.Ok(await ((CategoryDbService)db).GetCategoriesWithAllRelatedDataAsync());
    //    }
    //    catch
    //    {
    //    }

    //    return Results.BadRequest($"Couldn't get the requested products of type {typeof(Product).Name}.");
    //});
}

void RegisterServices()
{
    ConfigureAutoMapper();
    //Instances will be created by dependancy injection as needed a scoped instance is only allive for one request/response
    //singelton is allive during the life of the program.CategoryDb can be interchanged with any other class that implements IDbService
    //when the program asks for a IdbService, in this situation, it will get a CategoryDbService, that is what the code is saying.
    builder.Services.AddScoped<IDbService, CategoryDbService>(); 
                                                                 
}

void ConfigureAutoMapper()
{
    var config = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<Category, CategoryPostDTO>().ReverseMap();
        cfg.CreateMap<Category, CategoryPutDTO>().ReverseMap();
        cfg.CreateMap<Category, CategoryGetDTO>().ReverseMap();
        cfg.CreateMap<Category, CategorySmallGetDTO>().ReverseMap();
        /*cfg.CreateMap<Filter, FilterGetDTO>().ReverseMap();
        cfg.CreateMap<Size, OptionDTO>().ReverseMap();
        cfg.CreateMap<Color, OptionDTO>().ReverseMap();*/
    });
    var mapper = config.CreateMapper();
    builder.Services.AddSingleton(mapper);
}