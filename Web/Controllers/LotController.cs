using Bll.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Web.Models;

namespace Web.Controllers
{
    public class LotController : Controller
    {
        private readonly LotService _lotService;
        private readonly CategoryService _categoryService;
        private readonly LotImageService _lotImageService;
        private readonly IWebHostEnvironment _host;

        public LotController(LotService productService, CategoryService categoryService, LotImageService productImageService, IWebHostEnvironment webHost)
        {
            _lotService = productService;
            _lotImageService = productImageService;
            _categoryService = categoryService;
            _host = webHost;
        }

        public async Task<IActionResult> Index()
        {
            var res = await _lotService.FirstOrDefault(x => x.Id == 4);

            if (res.CloseTime >= DateTime.Now)
            {
                await _lotService.Expired(res.Id);
            }

            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.List();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LotViewModel lotView)
        {
            var time = DateTime.Now;
            switch (lotView.CloseTime)
            {
                case "1":
                    time.AddDays(1.0);
                    break;
                case "3":
                    time.AddDays(3.0);
                    break;
                case "7":
                    time.AddDays(7.0);
                    break;
                default:
                    break;
            }
            Lot lot = new()
            {
                Name = lotView.Name,
                Price = lotView.Price,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Description = lotView.Description,
                CategoryId = lotView.CategoryId,
                CloseTime = time           
            };
            var res = await _lotService.CreateAsync(lot);

            string filePath = Path.Combine(_host.WebRootPath, "Images");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (var uploadedFile in lotView.Url)
            {
                // путь к папке Files
                string path = "/Images/" + uploadedFile.FileName;

                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_host.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                LotImage img = new() { LotId = res.Id, Path = path };

                await _lotImageService.CreateAsync(img);
            }

            return RedirectToAction("RequiredProperty", "Product", new { res.CategoryId, ProductId = res.Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _lotImageService.DeleteFromServer(id);
            await _lotService.Delete(id);
            return RedirectToAction("Profile", "Seller");
        }

        public async Task<IActionResult> MakeBid(double bid, int productId)
        {
            bool res = await _lotService.MakeBid(bid, productId, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (res == false)
            {
                ViewBag.Error = "The bid must be greater than the price";
            }
            ViewBag.Error = "";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditDescription(string description, int lotId)
        {
            var lot = await _lotService.FirstOrDefault(l => l.Id == lotId);
            if (lot.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                await _lotService.Edit(description, lotId);
            }

            return RedirectToAction("Index");
        }
    }
}