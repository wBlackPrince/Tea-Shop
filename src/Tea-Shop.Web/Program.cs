using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Tea_Shop.Application.Database;
using Tea_Shop.Infrastructure.Postgres;
using Tea_Shop.Infrastructure.Postgres.Seeders;
using Tea_Shop.Infrastructure.S3;
using Tea_Shop.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddProgramDependencies();

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddScoped<ProductsDbContext>(_ => new ProductsDbContext(
    builder.Configuration.GetConnectionString("TeaShopDb")!));
builder.Services.AddScoped<IReadDbContext, ProductsDbContext>(_ => new ProductsDbContext(
    builder.Configuration.GetConnectionString("TeaShopDb")!));

builder.Services.Configure<MinioOptions>(builder.Configuration.GetSection("Minio"));
builder.Services.AddMinioDependencies(builder.Configuration);

var app = builder.Build();

//app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(
    options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tea-Shop v1"));

if (app.Environment.IsDevelopment())
{
    // для сидирования базы данных
    // if (args.Contains("--seeding"))
    // {
    //     await app.Services.RunSeeding();
    // }
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();