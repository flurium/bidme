using Bll.Models;
using Bll.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class AdminController : Controller
    {
        public static string Name => "admin";

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly BanService _banService;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, BanService banService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _banService = banService;
        }

        [Authorize(Roles = "")]
        public async Task<IActionResult> Index()
        {
            var roleExists = await _roleManager.RoleExistsAsync(Role.Admin);

            bool adminExists = false;

            if (roleExists)
            {
                var admins = await _userManager.GetUsersInRoleAsync(Role.Admin);
                adminExists = admins.Count > 0;
            }
            else
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.Admin));
            }

            if (adminExists) return View("Error");

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var res = await _userManager.AddToRoleAsync(user, Role.Admin);

            return res.Succeeded ? RedirectToAction(nameof(Index)) : View("Error");
        }

        public async Task<IActionResult> Users(UserViewModel filter)
        {
            var users = await _banService.FilterUsers(new UserFilter
            {
                Email = filter.Email ?? "",
                Id = filter.Id ?? "",
                Name = filter.Name ?? ""
            });

            var userViewModels = new List<UserViewModel>();
            foreach (var user in users)
            {
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.UserName,
                    IsBannedAsBuyer = await _userManager.IsInRoleAsync(user, Role.BannedAsBuyer),
                    IsBannedAsSeller = await _userManager.IsInRoleAsync(user, Role.BannedAsSeller),
                    IsAdmin = await _userManager.IsInRoleAsync(user, Role.Admin)
                });
            }

            ViewBag.Url = Request.Path + Request.QueryString;

            return View(userViewModels);
        }

        public async Task<IActionResult> MakeAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.AddToRoleAsync(user, Role.Admin);
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> UnmakeAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.RemoveFromRoleAsync(user, Role.Admin);
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> UnbanBuyer(string id, string redirect)
        {
            await _banService.UnbanBuyer(id);
            return Redirect(redirect);
        }

        public async Task<IActionResult> BanBuyer(string id, string redirect)
        {
            await _banService.BanBuyer(id);
            return Redirect(redirect);
        }

        public async Task<IActionResult> UnbanSeller(string id, string redirect)
        {
            await _banService.UnbanSeller(id);
            return Redirect(redirect);
        }

        public async Task<IActionResult> BanSeller(string id, string redirect)
        {
            await _banService.BanSeller(id);
            return Redirect(redirect);
        }
    }
}