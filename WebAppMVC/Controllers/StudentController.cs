using DomainCore.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers
{
    [Authorize(Roles = "Student")]
    [Route("[controller]")]
    public class StudentController : Controller
    {
        private JournalDBContext db;
        
        public StudentController(JournalDBContext context)
        {
            db = context;
        }

        #region MenuBar

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

        #endregion
    }
}