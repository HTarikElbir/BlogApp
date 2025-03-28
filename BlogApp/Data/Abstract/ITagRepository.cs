using BlogApp.Entities;

namespace BlogApp.Data.Abstract
{
    public interface ITagRepository
    {
        IQueryable<Tag> Tags { get; }
        void AddTag(Tag tag);

    }
}
