using System;
using System.Threading.Tasks;
using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ChatApp.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login() => View();
        [HttpPost]

        public async Task<IActionResult> Login(String username, String password)
        {
            User user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                SignInResult result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register() => View();
        [HttpPost]

        public async Task<IActionResult> Register(String username, String password)
        {
            User user = new User
            {
                UserName = username
            };

            IdentityResult result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Register", "Account");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}