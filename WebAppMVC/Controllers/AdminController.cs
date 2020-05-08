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
    [Authorize(Roles = "Administrator")]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private JournalDBContext db;

        public AdminController(ILogger<AdminController> logger, JournalDBContext context)
        {
            _logger = logger;
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
        #endregion

        #region AccountManage
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
                        AccountType = model.AccountType,
                        DateCreate = DateTime.Now,
                    };
                    if (!model.IsTermless && model.DateEnd != DateTime.MinValue)
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
        public async Task<IActionResult> EditAccount(int? id)
        {
            if (id != null)
            {
                Account account = await db.Accounts.FirstOrDefaultAsync(u => u.AccountId == id);
                if (account == null)
                    return RedirectToAction("Accounts");
                return View("Edit/Account", new AccountViewModel(account));
            }
            return RedirectToAction("Accounts");
        }
        [Route("EditAccount")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(AccountViewModel model)
        {
            Console.WriteLine(model.Id);
            if (ModelState.IsValid)
            {
                Account account = await db.Accounts.FirstOrDefaultAsync(u => u.AccountId == model.Id);
                if (account != null)
                {
                    account.LoginName = model.Login;
                    account.AccountType = model.AccountType;
                    if (!model.IsChangePassword)
                        account.Hpassword = DomainCore.Helpers.Password.Hash(model.Password);
                    if (!model.IsTermless && model.DateEnd != DateTime.MinValue)
                        account.DateEnd = model.DateEnd;
                    else
                        account.DateEnd = null;
                    db.Accounts.Update(account);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Accounts");
                }
                ModelState.AddModelError("", "Упс, что то пошло не так");
            }
            return View("Edit/Account", model);
        }

        [Route("RemoveAccount")]
        [HttpGet]
        public async Task<IActionResult> RemoveAccount(int? id)
        {
            if (id != null)
            {
                Account account = await db.Accounts.FirstOrDefaultAsync(u => u.AccountId == id);
                if (account == null)
                    return RedirectToAction("Accounts");
                db.Accounts.Remove(account);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Accounts");
        }
        #endregion

        #region StudentManage


        
        [Route("CreateStudent")]
        [HttpGet]
        public IActionResult CreateStudent()
        {
            return View("Create/Student");
        }
        
        [Route("CreateStudent")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStudent(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = await db.Accounts.FirstOrDefaultAsync(s => s.AccountId == model.Id || s.AccountType == "Student");
                if (account != null)
                {
                    Student student = new Student
                    {
                        AccountId = model.Id,
                        StudentSname = model.SecondName,
                        StudentName = model.FirstName,
                        StudentLname = model.LastName,
                        StudentGroup = model.Group
                    };
                    db.Students.Add(student);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Students");
                }
                ModelState.AddModelError("", "Аккаунта студента с таким индификатором не существует");
            }
            return View("Create/Student", model);
        }

        [Route("EditStudent")]
        [HttpGet]
        public async Task<IActionResult> EditStudent(int? id)
        {
            if (id != null)
            {
                Student student = await db.Students.FirstOrDefaultAsync(s => s.AccountId == id);
                if (student == null)
                    return RedirectToAction("Students");
                return View("Edit/Student", new StudentViewModel(student));
            }
            return RedirectToAction("Students");
        }
        [Route("EditStudent")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStudent(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = await db.Accounts.FirstOrDefaultAsync(s => s.AccountId == model.Id || s.AccountType == "Student");
                if (account != null)
                {
                    Student student = new Student
                    {
                        AccountId = model.Id,
                        StudentSname = model.SecondName,
                        StudentName = model.FirstName,
                        StudentLname = model.LastName,
                        StudentGroup = model.Group
                    };
                    db.Students.Update(student);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Students");
                }
                ModelState.AddModelError("", "Аккаунта студента с таким индификатором не существует");
            }
            return View("Edit/Student", model);
        }

        [Route("RemoveStudent")]
        [HttpGet]
        public async Task<IActionResult> RemoveStudent(int? id)
        {
            if (id != null)
            {
                Student student = await db.Students.FirstOrDefaultAsync(s => s.AccountId == id);
                if (student == null)
                    return RedirectToAction("Students");
                db.Students.Remove(student);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Students");
        }

        #endregion

        #region TeacherManage

        [Route("CreateTeacher")]
        [HttpGet]
        public IActionResult CreateTeacher()
        {
            return View("Create/Teacher");
        }

        [Route("CreateTeacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTeacher(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = await db.Accounts.FirstOrDefaultAsync(s => s.AccountId == model.Id || s.AccountType == "Teacher");
                if (account != null)
                {
                    Teacher teacher = new Teacher
                    {
                        AccountId = model.Id,
                        TeacherSname = model.SecondName,
                        TeacherName = model.FirstName,
                        TeacherLname = model.LastName
                    };
                    db.Teachers.Add(teacher);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Teachers");
                }
                ModelState.AddModelError("", "Аккаунта преподавателя с таким индификатором не существует");
            }
            return View("Create/Teacher", model);
        }

        [Route("EditTeacher")]
        [HttpGet]
        public async Task<IActionResult> EditTeacher(int? id)
        {
            if (id != null)
            {
                Teacher teacher = await db.Teachers.FirstOrDefaultAsync(s => s.AccountId == id);
                if (teacher == null)
                    return RedirectToAction("Teachers");
                return View("Edit/Teacher", new TeacherViewModel(teacher));
            }
            return RedirectToAction("Teachers");
        }
        [Route("EditTeacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTeacher(TeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = await db.Accounts.FirstOrDefaultAsync(s => s.AccountId == model.Id || s.AccountType == "Teacher");
                if (account != null)
                {
                    Teacher teacher = new Teacher
                    {
                        AccountId = model.Id,
                        TeacherSname = model.SecondName,
                        TeacherName = model.FirstName,
                        TeacherLname = model.LastName
                    };
                    db.Teachers.Update(teacher);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Teachers");
                }
                ModelState.AddModelError("", "Аккаунта преподавателя с таким индификатором не существует");
            }
            return View("Edit/Teacher", model);
        }

        [Route("RemoveTeacher")]
        [HttpGet]
        public async Task<IActionResult> RemoveTeacher(int? id)
        {
            if (id != null)
            {
                Teacher teacher = await db.Teachers.FirstOrDefaultAsync(s => s.AccountId == id);
                if (teacher == null)
                    return RedirectToAction("Teachers");
                db.Teachers.Remove(teacher);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Teachers");
        }

        #endregion

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyId(decimal id)
        {
            Student student = await db.Students.FirstOrDefaultAsync(s => s.AccountId == id);
            if (student != null)
                return Json($"На этот индификатор уже назначен студент");

            Teacher teacher = await db.Teachers.FirstOrDefaultAsync(s => s.AccountId == id);
            if (teacher != null)
                return Json($"На этот индификатор уже назначен преподаватель");

            Account account = await db.Accounts.FirstOrDefaultAsync(s => s.AccountId == id);
            if (account == null)
                return Json($"Аккаунта с таким индификатором не существует");

            return Json(true);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
