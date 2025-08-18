// nuget: Microsoft.EntityFrameworkCore.SqlServer
// nuget: Microsoft.EntityFrameworkCore.Tools
// dotnet tool install -g Microsoft.dotnet-openapi

using Microsoft.EntityFrameworkCore;
using garderie.app2.Server.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache(); // Required for session storage

var app = builder.Build();

app.UseSession();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
