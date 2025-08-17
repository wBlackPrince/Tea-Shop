using Tea_Shop.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProgramDependencies();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(
        options => options.SwaggerEndpoint("/openapi/v1.json", "Tea-Shop v1"));
}


app.MapControllers();

app.Run();