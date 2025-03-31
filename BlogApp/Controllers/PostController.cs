﻿using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        private IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
           
        }

        public async Task<IActionResult> Index(string tag)
        {
            var posts = _postRepository.Posts;
            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(p => p.Tags.Any(t => t.Url == tag));
            }
            return View(new PostViewModel
            {
                Posts = await posts.ToListAsync()
            });
        }

        public async Task<IActionResult> Details(string url)
        {
            var post = await _postRepository.Posts.FirstOrDefaultAsync(p => p.Url == url);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
    }
}
