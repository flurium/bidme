using Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    public class UserController : Controller
    {
        public static string Name => "user";

        private readonly LotService _lotService;
        private readonly OrderService _orderService;

        public UserController(LotService productService, OrderService orderService)
        {
            _lotService = productService;
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserLots()
        {
            ViewBag.Page = "Your Lots";
            var res = await _lotService.FindByConditionAsync(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View("Profile", res);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Archive()
        {
            ViewBag.Page = "Archive";
            var res = await _orderService.FindIncludeProductsAsync(x => (x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)) && (x.Lot.IsClosed == true));
            return View("Lots", res);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Bids()
        {
            ViewBag.Page = "Your bids";
            var res = await _orderService.FindIncludeProductsAsync(x => (x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)) && (x.Lot.IsClosed == false));
            return View("Lots", res);
        }
    }
}