﻿using BlogApp.Data.Abstract;
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

    }

}
