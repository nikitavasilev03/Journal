using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DomainCore.Context;
using DomainCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using WebAppMVC.Models;
using WebAppMVC.ViewModel;

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
            return View("Students", db.Students);
        }
        [Route("Teachers")]
        public IActionResult Teachers()
        {
            return View("Teachers", db.Teachers);
        }
        
        [Route("CreateAccount")]
        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View("Create/Account");
        }
        [Route("CreateAccount")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = await db.Accounts.FirstOrDefaultAsync(u => u.LoginName == model.Login);
                if (account == null)
                {
                    account = new Account
                    {
                        AccountId = db.NextSequence("SEQ_Accounts"),
                        LoginName = model.Login,
                        Hpassword = DomainCore.Helpers.Password.Hash(model.Password),
                        AccountType = "Ученик",
                        DateCreate = DateTime.Now,
                    };
                    if (model.DateEnd != DateTime.MinValue)
                        account.DateEnd = model.DateEnd;
                    db.Accounts.Add(account);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Accounts");
                }
                ModelState.AddModelError("", "Аккаунт с таким логином уже существует");
            }
            return View("Create/Account", model);
        }

        [Route("EditAccount")]
        [HttpGet]
        public IActionResult EditAccount()
        {
            return View("Edit/Account");
        }
        [Route("EditAccount")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = await db.Accounts.FirstOrDefaultAsync(u => u.LoginName == model.Login);
                if (account == null)
                {
                    account = new Account
                    {
                        AccountId = db.NextSequence("SEQ_Accounts"),
                        LoginName = model.Login,
                        Hpassword = DomainCore.Helpers.Password.Hash(model.Password),
                        AccountType = "Ученик",
                        DateCreate = DateTime.Now,
                    };
                    if (model.DateEnd != DateTime.MinValue)
                        account.DateEnd = model.DateEnd;
                    db.Accounts.Add(account);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Accounts");
                }
                ModelState.AddModelError("", "Аккаунт с таким логином уже существует");
            }

            return View("Edit/Account", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
