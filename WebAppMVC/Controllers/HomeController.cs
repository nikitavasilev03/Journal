using System.Diagnostics;
using DomainCore.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using WebAppMVC.Models;

namespace WebAppMVC.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private JournalDBContext db;

        public HomeController(ILogger<HomeController> logger, JournalDBContext context)
        {
            _logger = logger;
            db = context;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            return View("Profil");
        }
        [Route("Profil")]
        public IActionResult Profil()
        {
            return View("Profil");
        }
        [Route("Accounts")]
        public IActionResult Accounts()
        {
            return View("Accounts", db.Accounts);
        }
        [Route("Students")]
        public IActionResult Students()
        {
            return View("Students");
        }
        [Route("Teachers")]
        public IActionResult Teachers()
        {
            return View("Teachers");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
