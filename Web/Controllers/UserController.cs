using Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Lots()
        {
            ViewBag.Page = "I sell:";
            ViewBag.Route = $"{Request.Path}{Request.QueryString}";
            var res = await _lotService.FindByConditionAsync(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View("Profile", res);
        }

        [HttpGet]
        public async Task<IActionResult> Archive()
        {
            ViewBag.Page = "Archive";
            var res = await _orderService.UserOrders(User.FindFirstValue(ClaimTypes.NameIdentifier), true);
            return View("Lots", res);
        }

        [HttpGet]
        public async Task<IActionResult> Bids()
        {
            ViewBag.Page = "Your currently active bids";
            var res = await _orderService.UserOrders(User.FindFirstValue(ClaimTypes.NameIdentifier), false);
            return View("Lots", res);
        }
    }
}