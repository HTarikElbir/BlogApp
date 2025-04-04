using BlogApp.Data.Abstract;
using BlogApp.Entities;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if(User.Identity!.IsAuthenticated)
            {
                // If the user is already authenticated, redirect to the index page
                return RedirectToAction("Index", "Post");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
       

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid) 
            {
                // Check if the user exists in the database
                var isUser =  _userRepository.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                // If the user exists, create a claims identity and sign in
                if (isUser != null)
                {
                    var userClaims = new List<Claim>();
                    // Add claims for the user
                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Name, isUser.UserName ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.GivenName, isUser.Name ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.UserData, isUser.Image ?? ""));

                    // Add role claim if the user is an admin
                    if (isUser.Email == "info@lebronjames.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }
                    // Create a claims identity
                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperites = new AuthenticationProperties
                    {
                        IsPersistent = true, // This will keep the user logged in even after closing the browser

                    };  

                   
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperites);

                    return RedirectToAction("Index", "Post");
                }
                else
                {
                    // If the user does not exist, add a model error
                    ModelState.AddModelError("", "Invalid login attempt.");
                }

            }
           
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) 
            {
                // Check if the email already exists in the database
                var isUser = await _userRepository.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName || u.Email == model.Email);
                if (isUser == null) 
                {
                    // If the email does not exist, create a new user
                    _userRepository.AddUser(new User
                    {
                        UserName = model.UserName,
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        Image = "avatar.png"
                    });
                    // Save changes to the database
                    return RedirectToAction("Login");
                }
                else
                {
                    // If the email already exists, add a model error
                    ModelState.AddModelError("", "This email or username already exists.");
                }
    
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Profile(string username)
        {
            // Check if the username is null or empty
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }
            // Get the user by username
            var user = _userRepository
                .Users
                .Include(u => u.Posts)
                .Include(u => u.Comments)
                .ThenInclude(c => c.Post)
                .FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return NotFound();
            }
            
            return View(user);
        }



    }



}
