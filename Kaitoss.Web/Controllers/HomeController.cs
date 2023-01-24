using Kaitoss.Web.Data;
using Kaitoss.Web.Models;
using Kaitoss.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Kaitoss.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KaitossProjectContext _db;

        public HomeController(ILogger<HomeController> logger, KaitossProjectContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var tables = new TablesVM
            {
                Services = _db.Services.ToList(),
                Goals = _db.Goals.ToList(),
                About = _db.Abouts.ToList(),
                Information = _db.Informations.ToList(),
                Blog = _db.Blogs.ToList()

            };
            return View(tables);
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