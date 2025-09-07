using Tea_Shop.Application.Database;
using Tea_Shop.Infrastructure.Postgres;
using Tea_Shop.Infrastructure.S3;
using Tea_Shop.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProgramDependencies();
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddScoped<ProductsDbContext>(_ => new ProductsDbContext(
    builder.Configuration.GetConnectionString("TeaShopDb")!));
builder.Services.AddScoped<IReadDbContext, ProductsDbContext>(_ => new ProductsDbContext(
    builder.Configuration.GetConnectionString("TeaShopDb")!));

builder.Services.Configure<MinioOptions>(builder.Configuration.GetSection("Minio"));
builder.Services.AddMinioDependencies(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(
    options => options.SwaggerEndpoint("/openapi/v1.json", "Tea-Shop v1"));

if (app.Environment.IsDevelopment())
{
    // для сидирования базы данных
    // if (args.Contains("--seeding"))
    // {
    //     await app.Services.RunSeeding();
    // }
}


app.MapControllers();

app.Run();