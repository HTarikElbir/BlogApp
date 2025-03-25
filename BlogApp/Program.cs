using BlogApp.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

builder.Services.AddDbContext<BlogContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnetion")));

app.MapGet("/", () => "Hello World!");

app.Run();
