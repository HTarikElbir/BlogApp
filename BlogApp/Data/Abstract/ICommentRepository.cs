﻿using BlogApp.Entities;

namespace BlogApp.Data.Abstract
{
    public interface ICommentRepository
    {
        IQueryable<Comment> Comments { get; }
        void AddComment(Comment comment);

    }
}
