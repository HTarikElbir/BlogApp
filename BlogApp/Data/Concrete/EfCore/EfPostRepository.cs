using BlogApp.Data.Abstract;
using BlogApp.Entities;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfPostRepository : IPostRepository
    {
        private BlogContext _context;

        public EfPostRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Posts => _context.Posts;
        public void Create(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }
    }
}
