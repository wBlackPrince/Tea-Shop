using System.Text;
using Comments.Application;
using Commnets.Infrastructure.Postgres;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Minio;
using Orders.Application;
using Orders.Infrastructure.Postgres;
using Products.Application;
using Products.Infrastructure.Postgres;
using Subscriptions.Application;
using Subscriptions.Infrastructure.Postgres;
using Tea_Shop.Web;
using Users.Application;
using Users.Infrastructure.Postgres;

var builder = WebApplication.CreateBuilder(args);

// модули авторизации
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

// fluent email
builder.Services.AddHttpContextAccessor();
builder.Services
    .AddFluentEmail(builder.Configuration["Email:SenderEmail"], builder.Configuration["Email:Sender"])
    .AddSmtpSender(builder.Configuration["Email:Host"], builder.Configuration.GetValue<int>("Email:Port"));
Console.WriteLine($"SMTP Host: {builder.Configuration["Email:Host"]}");
Console.WriteLine($"SMTP Port: {builder.Configuration["Email:Port"]}");


Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;



// регистрируем DbContexts
builder.Services.AddScoped<CommentsDbContext>(_ => new CommentsDbContext(
    builder.Configuration.GetConnectionString("TeaShopDb")!));
builder.Services.AddScoped<ICommentsReadDbContext, CommentsDbContext>(_ => new CommentsDbContext(
    builder.Configuration.GetConnectionString("TeaShopDb")!));

builder.Services.AddScoped<OrdersDbContext>(_ => new OrdersDbContext(
    builder.Configuration.GetConnectionString("TeaShopDb")!));
builder.Services.AddScoped<IOrdersReadDbContext, OrdersDbContext>(_ => new OrdersDbContext(
    builder.Configuration.GetConnectionString("TeaShopDb")!));

builder.Services.AddScoped<ProductsDbContext>(_ => new ProductsDbContext(
    builder.Configuration.GetConnectionString("TeaShopDb")!));
builder.Services.AddScoped<IProductsReadDbContext, ProductsDbContext>(_ => new ProductsDbContext(
    builder.Configuration.GetConnectionString("TeaShopDb")!));

builder.Services.AddScoped<SubscriptionsDbContext>(_ => new SubscriptionsDbContext(
    builder.Configuration.GetConnectionString("TeaShopDb")!));
builder.Services.AddScoped<ISubscriptionsReadDbContext, SubscriptionsDbContext>(_ => new SubscriptionsDbContext(
    builder.Configuration.GetConnectionString("TeaShopDb")!));

builder.Services.AddScoped<UsersDbContext>(_ => new UsersDbContext(
    builder.Configuration.GetConnectionString("TeaShopDb")!));
builder.Services.AddScoped<IUsersReadDbContext, UsersDbContext>(_ => new UsersDbContext(
    builder.Configuration.GetConnectionString("TeaShopDb")!));

// Minio
builder.Services.Configure<MinioOptions>(builder.Configuration.GetSection("Infrastructure.Minio"));
builder.Services.AddMinioDependencies(builder.Configuration);

builder.Services.AddProgramDependencies();


var app = builder.Build();


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