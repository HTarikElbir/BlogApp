using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<IPostRepository, EfPostRepository>();

// Add services to the container.
builder.Services.AddDbContext<BlogContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Use static files
app.UseStaticFiles();
// Seed the database with some test data
SeedData.TestData(app);

app.MapDefaultControllerRoute();

app.Run();
