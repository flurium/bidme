using Bll.Models;
using Bll.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Web.Helpers;
using Web.Models;

namespace Web.Controllers
{
    public class LotController : Controller
    {
        public static string Name => "lot";

        private readonly LotService _lotService;
        private readonly CategoryService _categoryService;
        private readonly IWebHostEnvironment _host;
        private readonly FavoriteService _favoriteService;

        public LotController(LotService productService, CategoryService categoryService, IWebHostEnvironment webHost, FavoriteService favoriteService)
        {
            _lotService = productService;
            _categoryService = categoryService;
            _host = webHost;
            _favoriteService = favoriteService;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Main page of filter and serch for lots
        /// </summary>
        /// <param name="name">Search name of lot</param>
        /// <param name="category">One category name</param>
        /// <param name="min">Minimal price</param>
        /// <param name="max">Maximum price</param>
        public async Task<IActionResult> Index(string? name, double? min, double? max, string? category)
        {
            Category? categoryWithParents = category != null ? await _lotService.GetCategoryWithParents(category) : null;

            var subcategories = await _lotService.GetSubcategories(categoryWithParents?.Id);
            var filter = new LotFilter(name, categoryWithParents?.Id, min, max);

            var lots = await _lotService.FilterLots(filter);
            List<CatalogLotViewModel> lotViewModels = new(lots.Count);

            var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var lot in lots)
            {
                var isFavorite = await _favoriteService.IsExist(new Favorite(uid, lot.Id));
                lotViewModels.Add(new(lot, isFavorite));
            }

            var catalogViewModel = new CatalogViewModel
            {
                Category = categoryWithParents,
                Lots = lotViewModels,
                Subcategories = subcategories,
                Route = $"{Request.Path}{Request.QueryString}",
                Filter = filter
            };

            return View(catalogViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var res = await _lotService.GetDetails(id);

            if (res == null) return RedirectToAction(nameof(LotController.Index));

            if (res.CloseTime <= DateTime.Now && res.IsClosed == false)
            {
                await _lotService.Expired(res.Id);
                return RedirectToAction(nameof(LotController.Index));
            }

            var lastOrder = res.Orders.FirstOrDefault();

            var details = new LotDetailsViewModel(
                res.Id,
                res.Name,
                res.Images.Select(i => i.Path).ToArray(),
                res.Price,
                lastOrder != null ? lastOrder.OrderPrice : res.Price,
                res.CloseTime,
                res.IsClosed,
                res.Description,
               $"{Request.Path}{Request.QueryString}",
               isFavorite: await _favoriteService.IsExist(new Favorite(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
            );

            return View(details);
        }

        [HttpGet]
        [Authorize]
        [NotBannedAs(Role.BannedAsSeller)]
        public async Task<IActionResult> Create(string? category)
        {
            Category? categoryWithParents = category != null ? await _lotService.GetCategoryWithParents(category) : null;
            var subcategories = await _lotService.GetSubcategories(categoryWithParents?.Id);

            CreateLotViewModel viewModel = new(categoryWithParents, subcategories);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [NotBannedAs(Role.BannedAsSeller)]
        public async Task<IActionResult> Create(LotViewModel lotView)
        {
            var category = lotView.Category;
            if (category == null) return RedirectToAction(nameof(Create));

            var time = DateTime.Now;

            switch (lotView.CloseTime)
            {
                case "One":
                    time = time.AddDays(1.0);
                    break;

                case "Three":
                    time = time.AddDays(3.0);
                    break;

                case "Seven":
                    time = time.AddDays(7.0);
                    break;
            }

            var res = await _lotService.Create(new()
            {
                Name = lotView.Name,
                Price = lotView.Price,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                CloseTime = time,
                Description = lotView.Description,
                CategoryId = lotView.Category,
                NewCategoryName = lotView.NewCategory
            }, lotView.Url);

            return RedirectToAction(nameof(LotController.Index));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id, string redirect)
        {
            await _lotService.Delete(id);
            return Redirect(redirect);
        }

        [Authorize]
        [NotBannedAs(Role.BannedAsBuyer)]
        public async Task<IActionResult> MakeBid(double amount, int id)
        {
            var lot = await _lotService.GetDetails(id);
            if (lot.IsClosed == false)
            {
                bool res = await _lotService.MakeBid(amount, id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (res == false)
                {
                    ViewBag.Error = "The bid must be greater than the price";
                }
                ViewBag.Error = "";
                return RedirectToAction(nameof(Details), new { id });
            }
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        public async Task<IActionResult> EditDescription(string description, int lotId)
        {
            var lot = await _lotService.GetOne(lotId);
            if (lot != null && lot.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                await _lotService.Edit(description, lotId);
            }

            return RedirectToAction("Index");
        }
    }
}