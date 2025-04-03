using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class UserController : Controller
    {
        public UserController()
        {
            
        }

        public IActionResult Login()
        {
            return View();
        }

    }

}
