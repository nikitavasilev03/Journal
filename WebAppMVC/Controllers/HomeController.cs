using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (User.IsInRole("Administrator"))
                return RedirectToAction("Index", "Admin");
            if (User.IsInRole("Student"))
                return RedirectToAction("Index", "Student");
            if (User.IsInRole("Teacher"))
                return RedirectToAction("Index", "Teacher");
            return RedirectToAction("Index", "Start");
        }
    }
}