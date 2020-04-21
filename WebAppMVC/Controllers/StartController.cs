using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers
{
    public class StartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}