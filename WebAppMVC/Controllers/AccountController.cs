using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using WebAppMVC.ViewModel;
using DomainCore.Context;
using DomainCore.Models;
using System;

namespace WebAppMVC.Controllers
{
    public class AccountController : Controller
    {
        private JournalDBContext db;
        public AccountController(JournalDBContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = await db.Accounts.FirstOrDefaultAsync(u => u.LoginName == model.Login && u.Hpassword == DomainCore.Helpers.Password.Hash(model.Password));
                if (account == null)
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                else
                {
                    if (account.DateEnd != null && account.DateEnd <= DateTime.Now)
                        ModelState.AddModelError("", "Действие вашей учетной записи закончено");

                    switch (account.AccountType)
                    {
                        case "Student":
                            var student = await db.Students.FirstOrDefaultAsync(s => s.AccountId == account.AccountId);
                            if (student == null)
                                ModelState.AddModelError("", "Данный аккаунт не привязан");
                            break;
                        case "Teacher":
                            var teacher = await db.Teachers.FirstOrDefaultAsync(s => s.AccountId == account.AccountId);
                            if (teacher == null)
                                ModelState.AddModelError("", "Данный аккаунт не привязан");
                            break;
                    }

                    if (ModelState.ErrorCount == 0)
                    {
                        await Authenticate(account.LoginName, account.AccountType); // аутентификация
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(model);
        }


        private async Task Authenticate(string userName, string userRole)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Start");
        }
    }
}