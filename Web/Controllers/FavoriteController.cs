using Bll.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize]
    public class FavoriteController : Controller
    {
        public static string Name => "favorite";
        private readonly FavoriteService _favoriteService;

        public FavoriteController(FavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        public async Task<IActionResult> AddFavorite(int id, string redirect)
        {
            var favorite = new Favorite(User.FindFirstValue(ClaimTypes.NameIdentifier), id);
            if (!await _favoriteService.IsExist(favorite))
            {
                await _favoriteService.CreateAsync(favorite);
            }

            return Redirect(redirect);
        }

        public async Task<IActionResult> DeleteFavorite(int Id, string? redirect)
        {
            var favorite = new Favorite(User.FindFirstValue(ClaimTypes.NameIdentifier), Id);
            if (await _favoriteService.IsExist(favorite))
            {
                await _favoriteService.DeleteAsync(favorite);
            }
            if (redirect == null) return RedirectToAction(nameof(Index));
            return Redirect(redirect);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _favoriteService.FavoritesAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }
    }
}