using Dal.Models;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
                return RedirectToAction(nameof(LotController.Index), LotController.Name);
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
            if ((registerViewModel.Name.Length > 21) || registerViewModel.Name.Any(char.IsWhiteSpace))
            {
                error.Name = "The Name is too long (Max Length=20) and there must be no spaces in the Name";
                isError = true;
            }
            if (!registerViewModel.Email.Contains('@') || registerViewModel.Email.Length < 5)
            {
                error.Email = "Email must be more than 5 characters long and must contain '@'";
                isError = true;
            }
            if (
                registerViewModel.Password.Length < 6 ||
                !registerViewModel.Password.Any(char.IsUpper) ||
                !registerViewModel.Password.Any(char.IsLower) ||
                !registerViewModel.Password.Any(char.IsNumber) ||
                !registerViewModel.Password.Any(char.IsPunctuation)
                )
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
            return RedirectToAction(nameof(LotController.Index), LotController.Name);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangeName()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeName(string nameNew)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            user.UserName = nameNew;
            var res = await _userManager.UpdateAsync(user);

            if (res.Succeeded)
            {
                return RedirectToAction(nameof(UserController.Lots), UserController.Name);
            }
            return View("Error");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
            if (user == null)
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email, "Reset password token", $"Link-> <a href={callback}>Reset Password Link</a>");
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Token, resetPasswordViewModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}