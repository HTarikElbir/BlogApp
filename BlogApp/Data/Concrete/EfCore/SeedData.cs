using BlogApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public class SeedData
    {
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
                        new Tag { Text = "Web Programming" },
                        new Tag { Text = "Backend" },
                        new Tag { Text = "Frontend" },
                        new Tag { Text = "Fullstack" },
                        new Tag { Text = "Mobile" }
                        );
                    // Save the changes
                    context.SaveChanges();
                }
                // Check if there are any users in the database
                if (!context.Users.Any())
                {
                    // Add some users to the database
                    context.Users.AddRange( 
                        new User { UserName = "lebronjames"},
                        new User { UserName = "kobebryant" }
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
                            UserId = 1
                        },
                        new Post
                        {
                            Title = "Angular",
                            Content = "Angular Content",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-5),
                            Tags = context.Tags.Skip(2).Take(2).ToList(),
                            UserId = 2
                        },
                        new Post
                        {
                            Title = "React",
                            Content = "React Content",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-3),
                            Tags = context.Tags.Skip(1).Take(2).ToList(),
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
