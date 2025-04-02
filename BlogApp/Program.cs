using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<IPostRepository, EfPostRepository>();
builder.Services.AddScoped<ITagRepository, EfTagRepository>();

// Add services to the container.
builder.Services.AddDbContext<BlogContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Use static files
app.UseStaticFiles();
// Seed the database with some test data
SeedData.TestData(app);


app.MapControllerRoute(
    name: "posts_details",
    pattern: "posts/details/{url}",
    defaults: new { controller = "Post", action = "Details" }
);

app.MapControllerRoute(
    name: "posts_by_tag",
    pattern: "posts/tag/{tag}",
    defaults: new { controller = "Post", action = "Index" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Post}/{action=Index}/{id?}"
);



app.Run();
