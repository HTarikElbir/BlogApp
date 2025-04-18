﻿using BlogApp.Entities;

namespace BlogApp.Data.Abstract
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        void AddUser(User user);
    }

}
