using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Dal.Models;

namespace Web.Controllers
{
    public class EmailConfirmController : Controller
    {
        private readonly UserManager<User> _userManager;

        public EmailConfirmController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Confirm(string guid, string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user != null)
            {
                var res = await _userManager.ConfirmEmailAsync(user, guid);
                if (res.Succeeded)
                {
                    return View("ConfirmPage");
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}