using BlogApp.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

SeedData.TestData(app);

app.MapGet("/", () => "Hello World!");

app.Run();
