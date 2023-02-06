using Bll.Models;
using Bll.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public static string Name => "home";
        private readonly ILogger<HomeController> _logger;
        private readonly LotService lotService;

        public HomeController(ILogger<HomeController> logger, LotService lotService)
        {
            _logger = logger;
            this.lotService = lotService;
        }

        /// <summary>
        /// Main page of filter and serch for lots
        /// </summary>
        /// <param name="name">Search name of lot</param>
        /// <param name="categories">Comma seperated categories names</param>
        /// <param name="min">Minimal price</param>
        /// <param name="max">Maximum price</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(string? name, double? min, double? max, string categories = "")
        {
            var categoryList = categories.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            var catalogViewModel = new CatalogViewModel
            {
                Lots = await lotService.FilterLots(new LotFilter(name, categoryList, min, max)),
                Categories = await lotService.GetCategories(categoryList),
                SelectedCategories = categoryList
            };

            return View(catalogViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}