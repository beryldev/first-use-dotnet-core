using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Wrhs.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
           

  
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Auth(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            var result = await signInManager.PasswordSignInAsync(user, password, isPersistent: true, lockoutOnFailure: false);
            
            return RedirectToAction("Index", "Home");
        }
    }


    public class AuthModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}