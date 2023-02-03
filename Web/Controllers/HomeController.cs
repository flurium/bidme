using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public static string Name => "home";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var orderByPopularityLots = new List<Lot>()
            {
                new Lot {
                    Id=0,
                    Name="Lot 1",
                    Description="fakwjdsvn asdnv oasdof nanvoiansodnoasdn",
                },
                new Lot {
                    Id=1,
                    Name="Lot 2",
                    Description="nyt eneyn ene neynetrn",
                }
            };
            return View(orderByPopularityLots);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}