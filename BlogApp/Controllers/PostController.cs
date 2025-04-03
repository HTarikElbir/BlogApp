using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entities;
using BlogApp.Models;
using BlogApp.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var posts = _postRepository.Posts;

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
            var entity = new Comment
            {
                PostId = PostId,
                PublisedOn = DateTime.Now,
                Text = Text,
                User = new User { UserName = UserName, Image="avatar.jpg" }
            };
            _commentRepository.AddComment(entity);

            // Redirect to the post details page
            return Json(new
            {
                UserName,
                Text,
                entity.PublisedOn,
                entity.User.Image
            });
        } 
    }

}
