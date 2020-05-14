using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DomainCore.Context;
using DomainCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebAppMVC.Helpers;
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
        private static BufferController buffer = new BufferController();

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
            return View("Students", new StudentsSubjectsViewModel
            {
                Students = db.Students,
                Subjects = db.Subjects,
                Records = db.Records
            });
        }
        [Route("Teachers")]
        public IActionResult Teachers()
        {
            return View("Teachers", db.Teachers);
        }
        [Route("Subjects")]
        public IActionResult Subjects()
        {
            return View("Subjects", db.Subjects);
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
        public async Task<IActionResult> CreateTeacher(TeacherViewModel model)
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

        #region SubjectManage

        [Route("CreateSubject")]
        [HttpGet]
        public IActionResult CreateSubject()
        {
            return View("Create/Subject");
        }

        [Route("CreateSubject")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubject(SubjectViewModel model)
        {
            if (ModelState.IsValid)
            {

                Subject subject = new Subject
                {
                    SubjectId = db.NextSequence("SEQ_Subjects"),
                    SubjectName = model.Name,
                    NeedVisits = model.NeedVisits
                };
                db.Subjects.Add(subject);
                await db.SaveChangesAsync();


                return RedirectToAction("Subjects");
            }
            return View("Create/Subject", model);
        }

        [Route("EditSubject")]
        [HttpGet]
        public async Task<IActionResult> EditSubject(int? id)
        {
            if (id != null)
            {
                Subject subject = await db.Subjects.FirstOrDefaultAsync(s => s.SubjectId == id);
                if (subject == null)
                    return RedirectToAction("Subjects");
                return View("Edit/Subject", new SubjectViewModel(subject));
            }
            return RedirectToAction("Subjects");
        }
        [Route("EditSubject")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubject(SubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                Subject subject = await db.Subjects.FirstOrDefaultAsync(s => s.SubjectId == model.Id);
                if (subject != null)
                {
                    subject.SubjectName = model.Name;
                    subject.NeedVisits = model.NeedVisits;
                    db.Subjects.Update(subject);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Subjects");
                }
                ModelState.AddModelError("", "Предмета не существует");
            }
            return View("Edit/Subject", model);
        }

        [Route("RemoveSubject")]
        [HttpGet]
        public async Task<IActionResult> RemoveSubject(int? id)
        {
            if (id != null)
            {
                Subject subject = await db.Subjects.FirstOrDefaultAsync(s => s.SubjectId == id);
                if (subject == null)
                    return RedirectToAction("Subjects");
                db.Subjects.Remove(subject);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Subjects");
        }

        #endregion

        [Route("StudentSubjects")]
        [HttpGet]
        public async Task<IActionResult> StudentSubjects(int? stud)
        {
            if (stud != null)
            {
                Student student = await db.Students.FirstOrDefaultAsync(u => u.AccountId == stud);
                if (student == null)
                    return RedirectToAction("Studetns");

                var records = db.Records.Where(r => r.StudentAccountId == stud);
                var subjects = db.Subjects.Where(s => records.FirstOrDefault(r => r.SubjectId == s.SubjectId) != null);

                return View("StudentSubjects", new StudentsSubjectsViewModel
                {
                    Student = student,
                    Records = records,
                    Subjects = subjects
                });
            }
            return RedirectToAction("Studetns");
        }

        #region RecordManage

        [Route("AddRecord")]
        [HttpGet]
        public async Task<IActionResult> AddRecord(int? id)
        {
            Student student = await db.Students.FirstOrDefaultAsync(u => u.AccountId == id);
            if (student == null)
                return RedirectToAction("StudentSubjects",
                    new Dictionary<string, string>
                    {
                        {"stud", id?.ToString() }
                    });
            return View("Create/Record", new RecordViewModel
            {
                Student = student,
                Subjects = db.Subjects,
                DateEnd = DateTime.Now
            });
        }
        [Route("AddRecord")]
        [HttpPost]
        public async Task<IActionResult> AddRecord(RecordViewModel model, int? id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                    return RedirectToAction("StudentSubjects",
                    new Dictionary<string, string>
                    {
                        {"stud", id?.ToString() }
                    });

                var record = new Record
                {
                    RecordId = db.NextSequence("SEQ_Records"),
                    StudentAccountId = (decimal)id,
                    SubjectId = model.SubjectId,
                    DateStart = DateTime.Now,
                    DateEnd = model.DateEnd,
                    NumberVisits = 0,
                    IsPassed = false
                };
                db.Records.Add(record);
                await db.SaveChangesAsync();
                return RedirectToAction("StudentSubjects",
                    new Dictionary<string, string>
                    {
                        {"stud", id?.ToString() }
                    });
            }
            return View("Create/Record", model);
        }

        [Route("EditRecord")]
        [HttpGet]
        public async Task<IActionResult> EditRecord(int? id)
        {
            Record record = null;
            if (id != null)
                record = await db.Records.FirstOrDefaultAsync(u => u.RecordId == id);

            Student student = null;
            if (record != null)
                student = await db.Students.FirstOrDefaultAsync(u => u.AccountId == record.StudentAccountId);

            if (student == null)
                return RedirectToAction("StudentSubjects",
                    new Dictionary<string, string>
                    {
                        {"stud", id?.ToString() }
                    });

            RecordViewModel model = new RecordViewModel(record)
            {
                Student = student,
                Subjects = db.Subjects
            };

            return View("Edit/Record", model);
        }
        [Route("EditRecord")]
        [HttpPost]
        public async Task<IActionResult> EditRecord(RecordViewModel model, int? id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                    return RedirectToAction("Students",
                    new Dictionary<string, string>
                    {
                        {"id", id?.ToString() }
                    });

                Record record = await db.Records.FirstOrDefaultAsync(r => r.RecordId == id);
                if (record == null)
                    return RedirectToAction("StudentSubjects",
                    new Dictionary<string, string>
                    {
                        {"stud", record.StudentAccountId.ToString() }
                    });

                //record.SubjectId = model.SubjectId;
                record.DateEnd = model.DateEnd;

                db.Records.Update(record);
                await db.SaveChangesAsync();

                return RedirectToAction("StudentSubjects",
                    new Dictionary<string, string>
                    {
                        {"stud", record.StudentAccountId.ToString() }
                    });
            }
            return View("Create/Record", model);
        }

        [Route("RemoveRecord")]
        [HttpGet]
        public async Task<IActionResult> RemoveRecord(int? id)
        {
            if (id != null)
            {
                Record record = await db.Records.FirstOrDefaultAsync(r => r.RecordId == id);
                if (record == null)
                    return RedirectToAction("StudentSubjects",
                    new Dictionary<string, string>
                    {
                        {"stud", record.StudentAccountId.ToString() }
                    });

                db.Records.Remove(record);
                await db.SaveChangesAsync();

                return RedirectToAction("StudentSubjects",
                    new Dictionary<string, string>
                    {
                        {"stud", record.StudentAccountId.ToString() }
                    });
            }
            return RedirectToAction("StudentSubjects", "Students");
        }

        #endregion

        #region TimetableManage
        [Route("Timetable")]
        [HttpGet]
        public async Task<IActionResult> Timetable(int? teach, int? subj)
        {
            if (teach != null)
            {
                Teacher teacher = null;
                Subject subject = null;
                IQueryable<Timetable> timetable = null;
                IQueryable<Student> students = null;
                IQueryable<Record> records = null;
                IQueryable<Subject> subjects = db.Subjects;
                if (subj != null)
                {
                    teacher = db.Teachers.FirstOrDefault(u => u.AccountId == teach);
                    subject = db.Subjects.FirstOrDefault(s => s.SubjectId == subj);
                    if (teacher == null || subject == null)
                        return RedirectToAction("Teachers");

                    var tempRecords = db.Records.Where(r => r.SubjectId == subject.SubjectId);
                    timetable = db.Timetable.Where(tt => tempRecords.FirstOrDefault(r => r.RecordId == tt.RecordId) != null && tt.TeacherAccountId == teacher.AccountId);
                    records = tempRecords.Where(r => timetable.FirstOrDefault(tt => tt.RecordId == r.RecordId) != null);
                    students = db.Students.Where(s => records.FirstOrDefault(r => r.StudentAccountId == s.AccountId) != null);
                }
                else
                {
                    teacher = await db.Teachers.FirstOrDefaultAsync(u => u.AccountId == teach);
                    if (teacher == null)
                        return RedirectToAction("Teachers");
                    timetable = db.Timetable.Where(tt => tt.TeacherAccountId == teacher.AccountId);
                    records = db.Records.Where(r => timetable.FirstOrDefault(tt => tt.RecordId == r.RecordId) != null);
                }
                
                var model = new TeacherSubjectsViewModel
                {
                    CurrentTeacher = teacher,
                    CurrentSubject = subject,
                    Timetable = timetable,
                    Students = students,
                    Records = records,
                    Subjects = subjects
                };
                return View("Timetable", model);
            }
            return RedirectToAction("Teachers");
        }

        [Route("ShowTimetable")]
        [HttpPost]
        public IActionResult ShowTimetable(TeacherSubjectsViewModel model, int? id)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Teachers");
            if (id != null)
            {
                Teacher teacher = db.Teachers.FirstOrDefault(u => u.AccountId == id);
                Subject subject = db.Subjects.FirstOrDefault(s => s.SubjectId == model.CurrentSubjectId);
                if (teacher == null || subject == null)
                    return RedirectToAction("Teachers");

                var tempRecords = db.Records.Where(r => r.SubjectId == subject.SubjectId);
                var timetable = db.Timetable.Where(tt => tempRecords.FirstOrDefault(r => r.RecordId == tt.RecordId) != null && tt.TeacherAccountId == teacher.AccountId);
                var records = tempRecords.Where(r => timetable.FirstOrDefault(tt => tt.RecordId == r.RecordId) != null);
                var students = db.Students.Where(s => records.FirstOrDefault(r => r.StudentAccountId == s.AccountId) != null);

                model.CurrentTeacher = teacher;
                model.CurrentSubject = subject;
                model.Records = records;
                model.Timetable = timetable;
                model.Students = students;
                model.Subjects = db.Subjects;
                return View("Timetable", model);
            }
            return RedirectToAction("Teachers");
        }

        [Route("AddIntoTimeTable")]
        [HttpGet]
        public IActionResult AddIntoTimeTable(int teach, int subj)
        {
            Teacher teacher = db.Teachers.FirstOrDefault(u => u.AccountId == teach);
            Subject subject = db.Subjects.FirstOrDefault(s => s.SubjectId == subj);
            var records = db.Records.Where(r => r.SubjectId == subject.SubjectId);
            var students = db.Students.Where(s => records.FirstOrDefault(r => r.StudentAccountId == s.AccountId) != null);

            var model = new TeacherSubjectsViewModel
            {
                CurrentTeacher = teacher,
                CurrentSubject = subject,
                Timetable = null,
                Students = students,
                Records = records,
                Subjects = null
            };

            return View("Create/Timetable", model);
        }

        [HttpPost]
        [Route("AddRecordToTimeTable")]
        public void AddRecordToTimeTable(int teacherId, int recordId, int dayOfWeek, int numberLesson)
        {
            Timetable ttRecord = new Timetable
            {
                TeacherAccountId = teacherId,
                RecordId = recordId,
                TtWeekDay = dayOfWeek,
                TtNumLesson = numberLesson
            };
            if (db.Timetable.FirstOrDefault(tt => tt.TeacherAccountId == ttRecord.TeacherAccountId
                    && tt.RecordId == ttRecord.RecordId
                    && tt.TtWeekDay == ttRecord.TtWeekDay
                    && tt.TtNumLesson == ttRecord.TtNumLesson) == null)
            {
                ttRecord.TtId = db.NextSequence("SEQ_Timetable");
                db.Timetable.Add(ttRecord);
                db.SaveChanges();
            }
        }

        [HttpGet]
        [Route("EditTimetable")]
        public IActionResult EditTimetable(int teach, int subj, int rec)
        {
            Timetable tt = db.Timetable.FirstOrDefault(t => t.TtId == rec);
            TimetableRecordViewModel model = new TimetableRecordViewModel(tt);
            model.SubjectId = subj;
            return View("Edit/Timetable", model);
        }

        [HttpPost]
        [Route("EditTimetable")]
        public async Task<IActionResult> EditTimetable(TimetableRecordViewModel model, int teach, int subj, int rec)
        {
            if (ModelState.IsValid)
            {
                Timetable ttRecord = db.Timetable.FirstOrDefault(t => t.TtId == rec);
                ttRecord.TtNumLesson = model.NumberLesson;
                ttRecord.TtWeekDay = model.DayOfWeek;
                if (db.Timetable.FirstOrDefault(tt => tt.TeacherAccountId == ttRecord.TeacherAccountId
                   && tt.RecordId == ttRecord.RecordId
                   && tt.TtWeekDay == ttRecord.TtWeekDay
                   && tt.TtNumLesson == ttRecord.TtNumLesson) == null)
                {
                    db.Timetable.Update(ttRecord);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Timetable", new Dictionary<string, string> {
                        { "teach", teach.ToString() },
                        { "subj", subj.ToString() }
                    });
                }
                ModelState.AddModelError("", "Данная запись уже есть");
            }
            return View("Edit/Timetable", model);
        }
        [HttpGet]
        [Route("RemoveTimetable")]
        public async Task<IActionResult> RemoveTimetable(int teach, int subj, int rec)
        {
            Timetable ttRecord = db.Timetable.FirstOrDefault(t => t.TtId == rec);
            db.Timetable.Remove(ttRecord);
            await db.SaveChangesAsync();
            return RedirectToAction("Timetable", new Dictionary<string, string> {
                        { "teach", teach.ToString() },
                        { "subj", subj.ToString() }
                    });
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
