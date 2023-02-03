using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Dal.Models;

using Web.Controllers;
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

    //public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IEmailSender emailSender, RoleManager<IdentityRole> roleManager)
    //{
    //  _signInManager = signInManager;
    //  _userManager = userManager;
    //  _emailSender = emailSender;
    //  _roleManager = roleManager;
    //}

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
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
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