using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entities;
using BlogApp.Models;
using BlogApp.ViewComponents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        private IPostRepository _postRepository;
        private ICommentRepository _commentRepository;

        // Constructor
        public PostController(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        //  Display the list of posts
        public async Task<IActionResult> Index(string tag)
        {
            //  Get the list of posts
            var posts = _postRepository.Posts.Where(p => p.IsActive);

            //  If the tag is not empty, filter the posts by tag
            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(p => p.Tags.Any(t => t.Url == tag));
            }
            //  Return the view with the list of posts
            return View(new PostViewModel
            {
                Posts = await posts.ToListAsync()
            });
        }
        //  Display the post details
        public async Task<IActionResult> Details(string url)
        {
            //  Include the tags and comments in the post
            var post = await _postRepository
                .Posts
                .Include(p => p.User)
                .Include(p => p.Tags)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.Url == url);

            //  If the post is not found, return a 404 error
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
        //  Add comment to the database
        [HttpPost]
        public JsonResult AddComment(int PostId, string UserName, string Text, string Url)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);

            var entity = new Comment
            {
                PostId = PostId,
                Text = Text,
                PublisedOn = DateTime.Now,
                UserId = int.Parse(userId ?? "")
            };
            _commentRepository.AddComment(entity);

            // Redirect to the post details page
            return Json(new
            {
                username,
                Text,
                entity.PublisedOn,
                avatar
            });
        }


        //  Display the create post form
        [Authorize] 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //  Create a new post and add it to the database
                var post = new Post
                {
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url,
                    UserId = int.Parse(userId ?? ""),
                    PublishedOn = DateTime.Now,
                    Image = "1.jpg",
                    IsActive = false
                };
                _postRepository.Create(post);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> List()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            var posts = _postRepository.Posts;

            if (string.IsNullOrEmpty(role))
            {
                posts = posts.Where(p => p.UserId == userId);
            }
            return View(await posts.ToListAsync());
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if(id== null)
            {
                return NotFound();
            }

            var post = _postRepository.Posts.FirstOrDefault(p => p.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(new PostCreateViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Url = post.Url,
                IsActive = post.IsActive,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entityUpdate = new Post
                {
                    PostId = model.PostId,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url,
                };
                if (User.FindFirstValue(ClaimTypes.Role) == "admin")
                {
                    entityUpdate.IsActive = model.IsActive;
                }

                _postRepository.Edit(entityUpdate);
                return RedirectToAction("List");
            }

           
            return View(model);
        }
    }

}
