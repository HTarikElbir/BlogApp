using BlogApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public class SeedData
    {
        // Method to seed the database with some test data
        public static void TestData(IApplicationBuilder app)
        {
            // Get the BlogContext from the application services
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();


            if (context != null)
            {
                // Apply any pending migrations
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
                // Check if there are any tags in the database
                if (!context.Tags.Any())
                {
                    // Add some tags to the database
                    context.Tags.AddRange(
                        new Tag { Text = "Web Programming", Url="web-programming", Colour = TagColour.success},
                        new Tag { Text = "Backend", Url = "backend", Colour = TagColour.danger },
                        new Tag { Text = "Frontend", Url = "frontend", Colour = TagColour.secondary },
                        new Tag { Text = "Fullstack",Url = "fullstack", Colour = TagColour.dark },
                        new Tag { Text = "Mobile", Url="mobile", Colour = TagColour.light}
                        );
                    // Save the changes
                    context.SaveChanges();
                }
                // Check if there are any users in the database
                if (!context.Users.Any())
                {
                    // Add some users to the database
                    context.Users.AddRange( 
                        new User { UserName = "lebronjames", Name = "Lebron", Email = "info@lebronjames.com", Password = "123456",Image = "p1.jpg"},
                        new User { UserName = "kobebryant", Name = "Kobe", Email = "info@kobe.com", Password = "123456" ,Image = "p2.jpg" }
                    );
                    // Save the changes
                    context.SaveChanges();
                }
                // Check if there are any posts in the database
                if (!context.Posts.Any())
                {
                    // Add some posts to the database
                    context.Posts.AddRange(
                        new Post
                        {
                            Title = "ASP.NET Core",
                            Content = "ASP.NET Core Content",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "1.jpg",
                            Url = "aspnet-core",
                            UserId = 1,
                            Comments = new List<Comment> { 
                                new Comment { Text= "Good", PublisedOn = new DateTime(), UserId = 1 },
                                new Comment { Text= "Awesome", PublisedOn = new DateTime(), UserId = 2 }}
                        },
                        new Post
                        {
                            Title = "Angular",
                            Content = "Angular Content",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-5),
                            Tags = context.Tags.Skip(2).Take(2).ToList(),
                            Image = "2.jpg",
                            Url = "angular",
                            UserId = 2
                        },
                        new Post
                        {
                            Title = "React",
                            Content = "React Content",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-3),
                            Tags = context.Tags.Skip(1).Take(2).ToList(),
                            Image = "3.jpg",
                            Url = "react",
                            UserId = 1
                        },
                        new Post
                        {
                            Title = "Vue",
                            Content = "Vue Content",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-1),
                            Tags = context.Tags.Skip(1).Take(2).ToList(),
                            Image = "2.jpg",
                            Url = "vue",
                            UserId = 2
                        },
                        new Post
                        {
                            Title = "Xamarin",
                            Content = "Xamarin Content",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-1),
                            Tags = context.Tags.Skip(4).Take(1).ToList(),
                            Image = "1.jpg",
                            Url = "xamarin",
                            UserId = 1
                        }
                    );
                    // Save the changes
                    context.SaveChanges();
                }
            }
        }
        
    }
}
