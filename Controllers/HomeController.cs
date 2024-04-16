using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DemoGPLX.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace DemoGPLX.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies["DataUserGPLX"] != null)
            {
                DataUser dtu = JsonConvert.DeserializeObject<DataUser>(HttpContext.Request.Cookies["DataUserGPLX"]);
                ViewBag.hang = new DbGplxContext().Hangs.ToList();
                if (dtu != null)
                    return View(dtu);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}