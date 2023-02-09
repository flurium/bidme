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

        public LotController(LotService productService, CategoryService categoryService, IWebHostEnvironment webHost)
        {
            _lotService = productService;
            _categoryService = categoryService;
            _host = webHost;
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
        /// <param name="categories">Comma seperated categories names</param>
        /// <param name="min">Minimal price</param>
        /// <param name="max">Maximum price</param>
        public async Task<IActionResult> Index(string? name, double? min, double? max, string categories = "")
        {
            var categoryList = categories.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            var catalogViewModel = new CatalogViewModel
            {
                Lots = await _lotService.FilterLots(new LotFilter(name, categoryList, min, max)),
                Categories = await _lotService.GetCategories(categoryList),
                SelectedCategories = categoryList
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
                res.Description
            );

            return View(details);
        }

        [HttpGet]
        [Authorize]
        [NotBannedAs(Roles = Role.BannedAsSeller)]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.List();
            return View();
        }

        [HttpPost]
        [Authorize]
        [NotBannedAs(Roles = Role.BannedAsSeller)]
        public async Task<IActionResult> Create(LotViewModel lotView)
        {
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

            Lot lot = new()
            {
                Name = lotView.Name,
                Price = lotView.Price,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                CloseTime = time,
                Description = lotView.Description,
                CategoryId = lotView.CategoryId
            };
            var res = await _lotService.Create(lot, lotView.Url);

            return RedirectToAction(nameof(LotController.Index));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _lotService.Delete(id);
            return RedirectToAction("Profile", "Seller");
        }

        [Authorize]
        [NotBannedAs(Roles = Role.BannedAsBuyer)]
        public async Task<IActionResult> MakeBid(double amount, int id)
        {
            bool res = await _lotService.MakeBid(amount, id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (res == false)
            {
                ViewBag.Error = "The bid must be greater than the price";
            }
            ViewBag.Error = "";
            return RedirectToAction("Index");
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