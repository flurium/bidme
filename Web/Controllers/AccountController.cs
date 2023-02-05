using Dal.Models;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        public static string Name => "account";

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IEmailSender emailSender, RoleManager<IdentityRole> roleManager)
        {
          _signInManager = signInManager;
         _userManager = userManager;
         _emailSender = emailSender;
          _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            var res = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, true, false);
            if (res.Succeeded)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View("Error");
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Error = new RegisterViewModel();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var error = new RegisterViewModel();
            bool isError = false;
            if ((registerViewModel.Name.Length > 21) || (registerViewModel.Name.Any(Char.IsWhiteSpace))) 
            {
                error.Name = "The Name is too long (Max Length=20) and there must be no spaces in the Name";
                isError = true;
            }
            if (!registerViewModel.Email.Contains("@") || registerViewModel.Email.Length < 5)
            {
                error.Email = "Email must be more than 5 characters long and must contain '@'";
                isError = true;
            }
            if ((!registerViewModel.Password.Any(Char.IsUpper)) || (!registerViewModel.Password.Any(Char.IsLower)) || (!registerViewModel.Password.Any(Char.IsNumber)) || (registerViewModel.Password.Length < 6) || (!registerViewModel.Password.Any(Char.IsPunctuation)))
            {
                error.Password = "Password must be at least 6 characters, at least one A-Z, a-z and a special character";
                isError = true;
            }
            if (registerViewModel.Password != registerViewModel.ConfirmPassword)
            {
                error.ConfirmPassword = "Your passwords are different";
                isError = true;
            }

            if (isError)
            {
                ViewBag.Error = error;
                return View();
            }

            var user = new User()
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Name,
            };

            var res = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (res.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("", "confirmation", new { guid = token, userEmail = user.Email }, Request.Scheme, Request.Host.Value);
                await _emailSender.SendEmailAsync(user.Email, "ConfirmationLink", $"Link-> <a href={confirmationLink}>Confirmation Link</a>");
                ViewBag.Email = user.Email;
                return View("Confirmation");
            }

            return View("Error");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}