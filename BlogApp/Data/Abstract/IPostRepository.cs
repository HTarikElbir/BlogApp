using BlogApp.Entities;

namespace BlogApp.Data.Abstract
{
    public interface IPostRepository
    {
        IQueryable<Post> Posts { get; }
        void Create(Post post);

        void Edit(Post post);
    }
}
