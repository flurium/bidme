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
        private readonly FavoriteService _favoriteService;

        public FavoriteController(FavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }
        public async Task<IActionResult> AddFavorite(int Id)
        {
           
            var favorite = new Favorite(User.FindFirstValue(ClaimTypes.NameIdentifier), Id);
            if (!await _favoriteService.IsExist(favorite))
            {
            await _favoriteService.CreateAsync(favorite);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteFavorite(int Id)
        {
            var favorite = new Favorite(User.FindFirstValue(ClaimTypes.NameIdentifier), Id);
            if (await _favoriteService.IsExist(favorite))
            {
                await _favoriteService.DeleteAsync(favorite);
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Index()
        {
            return View(await _favoriteService.FavoritesAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }
    }
}
