using Tea_Shop.Infrastructure.Postgres;
using Tea_Shop.Infrastructure.Postgres.Seeders;
using Tea_Shop.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProgramDependencies();
builder.Services.AddScoped<ProductsDbContext>(_ => new ProductsDbContext(
    builder.Configuration.GetConnectionString("TeaShopDb")!));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(
        options => options.SwaggerEndpoint("/openapi/v1.json", "Tea-Shop v1"));

    // для сидирования базы данных
    // if (args.Contains("--seeding"))
    // {
    //     await app.Services.RunSeeding();
    // }
}


app.MapControllers();

app.Run();