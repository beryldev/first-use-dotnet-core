using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Wrhs.WebApp.ViewModels;

namespace Wrhs.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IdentityDbContext dbContext;

        public AccountController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager, IdentityDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if(!dbContext.Users.Any(u=>u.UserName=="admin"))
                return RedirectToAction("Password");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Auth(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);

            if(user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, password, isPersistent: true, lockoutOnFailure: false);
                if(result.Succeeded)
                    return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login");        
        }

        [HttpGet]
        public IActionResult Password()
        {
            if(!dbContext.Users.Any(u=>u.UserName=="admin"))
                return View(new PasswordViewModel());

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Password(string password)
        {
            if(!string.IsNullOrEmpty(password) && !dbContext.Users.Any(u=>u.UserName=="admin"))
            {
                var result = await userManager.CreateAsync(new IdentityUser
                {
                    UserName = "admin"
                }, password);

                if(!result.Succeeded)
                {
                    var vm = new PasswordViewModel
                    {
                        Errors = result.Errors.Select(x=>x.Description).ToList()
                    };

                    return View(vm);
                }         
            }

            return RedirectToAction("Login");       
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}