using DomainCore.Context;
using DomainCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebAppMVC.ViewModel;

namespace WebAppMVC.Controllers
{
    [Authorize(Roles = "Student")]
    [Route("[controller]")]
    public class StudentController : Controller
    {
        private JournalDBContext db;
        private Student currentUser = null;

        public Student CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    var login = User.Identity.Name;
                    currentUser = db.Students.FirstOrDefault(
                        t => t.AccountId == db.Accounts.FirstOrDefault(a => a.LoginName == login).AccountId
                    );
                }
                return currentUser;
            }
        }

        public StudentController(JournalDBContext context)
        {
            db = context;
        }

        #region MenuBar
        
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return View("Profil");
        }
        
        [HttpGet]
        [Route("Profil")]
        public IActionResult Profil()
        {
            return View("Profil");
        }

        [HttpGet]
        [Route("Attendance")]
        public IActionResult Attendance()
        {
            Student student = CurrentUser;
            var records = db.Records.Where(r => r.StudentAccountId == student.AccountId);
            var subjects = db.Subjects.Where(s => records.FirstOrDefault(r => r.SubjectId == s.SubjectId) != null);

            StudentAttendanceViewModel model = new StudentAttendanceViewModel
            {
                CurrentStudent = student,
                CurrentSubject = null,
                Records = records,
                Subjects = subjects,
                Journals = null,
                Teachers = null
            };

            return View("Attendance", model);
        }

        [HttpPost]
        [Route("Attendance")]
        public IActionResult Attendance(StudentAttendanceViewModel model)
        {
            Student student = CurrentUser;
            Subject subject = db.Subjects.FirstOrDefault(s => s.SubjectId == model.CurrentSubjectId);
 
            var records = db.Records.Where(r => r.StudentAccountId == student.AccountId);
            var subjects = db.Subjects.Where(s => records.FirstOrDefault(r => r.SubjectId == s.SubjectId) != null);
            var journals = db.Journals.Where(j => j.SubjectId == subject.SubjectId && j.StudentAccountId == student.AccountId);
            var teachers = db.Teachers.Where(t => journals.FirstOrDefault(j => j.TeacherAccountId == t.AccountId) != null);

            model.CurrentStudent = student;
            model.CurrentSubject = subject;
            model.Records = records;
            model.Subjects = subjects;
            model.Journals = journals;
            model.Teachers = teachers;

            return View("Attendance", model);
        }

        #endregion
    }
}