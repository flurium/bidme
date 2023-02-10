using Bll.Models;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bll.Services
{
    public class BanService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<User> _signInManager;

        public BanService(UserManager<User> userManager, IEmailSender emailSender, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IEnumerable<User>> FilterUsers(UserFilter filter)
        {
            List<Expression<Func<User, bool>>> predicates = new();

            if (filter.Name != "") predicates.Add(u => u.UserName.StartsWith(filter.Name));
            if (filter.Email != "") predicates.Add(u => u.Email.StartsWith(filter.Email));
            if (filter.Id != "") predicates.Add(u => u.Id.StartsWith(filter.Id));

            var users = _userManager.Users;

            foreach (var predicate in predicates)
            {
                users = users.Where(predicate);
            }

            return await users.ToListAsync();
        }

        public async Task UnbanBuyer(string uid)
        {
            var user = await _userManager.FindByIdAsync(uid);
            var res = await _userManager.RemoveFromRoleAsync(user, Role.BannedAsBuyer);
            if (res.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                await _emailSender.SendEmailAsync(user.Email, "Unbanned in BidMe", "You were unbanned in BidMe!");
            }
        }

        public async Task BanBuyer(string uid)
        {
            var user = await _userManager.FindByIdAsync(uid);
            if (!await _roleManager.RoleExistsAsync(Role.BannedAsBuyer)) await _roleManager.CreateAsync(new IdentityRole(Role.BannedAsBuyer));

            var res = await _userManager.AddToRoleAsync(user, Role.BannedAsBuyer);
            if (res.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                await _emailSender.SendEmailAsync(user.Email, "Banned in BidMe",
                    "YOU ARE BANNED IN BIDME! You did bad actions. Reply to this email with apologies to get unbanned!");
            }
        }

        public async Task UnbanSeller(string uid)
        {
            var user = await _userManager.FindByIdAsync(uid);
            var res = await _userManager.RemoveFromRoleAsync(user, Role.BannedAsSeller);
            if (res.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                await _emailSender.SendEmailAsync(user.Email, "Unbanned in BidMe", "You were unbanned in BidMe! Now u may sell");
            }
        }

        public async Task BanSeller(string uid)
        {
            var user = await _userManager.FindByIdAsync(uid);
            if (!await _roleManager.RoleExistsAsync(Role.BannedAsSeller)) await _roleManager.CreateAsync(new IdentityRole(Role.BannedAsSeller));

            var res = await _userManager.AddToRoleAsync(user, Role.BannedAsSeller);
            if (res.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                await _emailSender.SendEmailAsync(user.Email, "Banned in BidMe",
                    "YOU ARE BANNED IN BIDME! You did bad actions. Reply to this email with apologies to get unbanned and sell again");
            }
        }
    }
}