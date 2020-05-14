using DomainCore.Context;
using DomainCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppMVC.ViewModel;

namespace WebAppMVC.Controllers
{
    [Authorize(Roles = "Teacher")]
    [Route("[controller]")]
    public class TeacherController : Controller
    {
        private JournalDBContext db;
        private Teacher currentUser = null;
        private IEnumerable<Timetable> currnetTimetable = null;
        private IEnumerable<Journal> currnetAttendance = null;

        public Teacher CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    var login = User.Identity.Name;
                    currentUser = db.Teachers.FirstOrDefault(
                        t => t.AccountId == db.Accounts.FirstOrDefault(a => a.LoginName == login).AccountId
                    );
                }
                return currentUser;
            }
        }

        public IEnumerable<Timetable> CurrnetTimetable
        {
            get
            {
                if (currnetTimetable == null)
                    currnetTimetable = db.Timetable.Where(t => t.TeacherAccountId == CurrentUser.AccountId);
                return currnetTimetable;
            }
        }

        public IEnumerable<Journal> CurrnetAttendance
        {
            get
            {
                if (currnetAttendance == null)
                    currnetAttendance = db.Journals.Where(a => a.TeacherAccountId == CurrentUser.AccountId);
                return currnetAttendance;
            }
        }

        public TeacherController(JournalDBContext context)
        {
            db = context;
        }

        #region MenuBar

        [Route("Index")]
        public IActionResult Index()
        {
            return RedirectToAction("Profil");
        }
        [Route("Profil")]
        public IActionResult Profil()
        {
            return View("Profil", CurrentUser);
        }

        [HttpGet]
        [Route("Timetable")]
        public IActionResult Timetable()
        {
            var timetable = CurrnetTimetable;
            var records = db.Records.Where(r => timetable.FirstOrDefault(tt => tt.RecordId == r.RecordId) != null);
            var subjects = db.Subjects.Where(s => records.FirstOrDefault(r => r.SubjectId == s.SubjectId) != null);

            TimetableViewModel model = new TimetableViewModel
            {
                Timetable = timetable,
                Records = records,
                Subjects = subjects
            };

            return View("Timetable", model);
        }

        [HttpGet]
        [Route("Journal")]
        public IActionResult Journal()
        {
            var records = db.Records.Where(r => CurrnetTimetable.FirstOrDefault(t => t.RecordId == r.RecordId) != null);
            var subjects = db.Subjects.Where(s => records.FirstOrDefault(t => t.SubjectId == s.SubjectId) != null);

            if (subjects.Count() == 0)
                return View("Journal", null);

            JournalViewModel model = new JournalViewModel
            {
                Subjects = subjects,
                CurrentSubjectId = subjects.FirstOrDefault().SubjectId,
                Day = DateTime.Now
            };

            return View("Journal", model);
        }

        [HttpPost]
        [Route("ShowJournal")]
        public IActionResult ShowJournal(JournalViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Journal");

            //Занятие преподователя в текущее время
            var timetable = CurrnetTimetable
                .Where(t => t.TtNumLesson == model.NumberLeson && t.TtWeekDay == (int)model.Day.DayOfWeek);
            //Если занятия нет
            if (timetable.Count() == 0)
            {
                var subjects = db.Subjects.
                    Where(s => CurrnetAttendance.FirstOrDefault(a => a.SubjectId == s.SubjectId) != null);
                model.Subjects = subjects;
                ModelState.AddModelError("", "У вас нет занятий в это время");
                return View("Journal", model);
            }
            //Записи на занятие в текущее время
            var records = db.Records.AsEnumerable()
                .Where(r => timetable.FirstOrDefault(t => t.RecordId == r.RecordId) != null && r.SubjectId == model.CurrentSubjectId);
            //Если никто не записан
            if (timetable.Count() == 0)
            {
                var subjects = db.Subjects.
                    Where(s => CurrnetAttendance.FirstOrDefault(a => a.SubjectId == s.SubjectId) != null);
                model.Subjects = subjects;
                ModelState.AddModelError("", "Нет записей на данное занятие");
                return View("Journal", model);
            }

            //Студенты на текущее время
            model.Students = db.Students.AsEnumerable()
                .Where(s => records.FirstOrDefault(r => r.StudentAccountId == s.AccountId) != null);
            //Предметы, если потребуется выбрать другой предмет
            model.Subjects = db.Subjects.AsEnumerable()
               .Where(s => CurrnetAttendance.FirstOrDefault(a => a.SubjectId == s.SubjectId) != null);

            model.Journals = db.Journals.AsEnumerable()
                .Where(j => model.Students.FirstOrDefault(s => s.AccountId == j.StudentAccountId) != null &&
                        j.SubjectId == model.CurrentSubjectId &&
                        j.TeacherAccountId == CurrentUser.AccountId &&
                        j.VisitDate == model.Day
                        );

            return View("Journal", model);
        }

        [HttpPost]
        [Route("JournalAdd")]
        public void JournalAdd(int subjectId, int studentId, DateTime date)
        {
            Journal journal = new Journal
            {
                JourId = db.NextSequence("SEQ_Journals"),
                StudentAccountId = studentId,
                SubjectId = subjectId,
                TeacherAccountId = CurrentUser.AccountId,
                VisitDate = date
            };
            db.Journals.Add(journal);
            db.SaveChanges();
        }

        [HttpPost]
        [Route("JournalRemove")]
        public void JournalRemove(int subjectId, int studentId, DateTime date)
        {
            var journal = db.Journals.FirstOrDefault(
                j => j.StudentAccountId == studentId &&
                     j.SubjectId == subjectId &&
                     j.VisitDate.Value.Date == date.Date
            );

            if (journal == null)
                return;

            db.Journals.Remove(journal);
            db.SaveChanges();
        }

        #endregion

        [HttpGet]
        [Route("Attendance")]
        public IActionResult Attendance(decimal? gr, decimal? subj)
        {
            var timetable = db.Timetable.Where(t => t.TeacherAccountId == CurrentUser.AccountId);
            var records = db.Records.Where(r => timetable.FirstOrDefault(t => t.RecordId == r.RecordId) != null);
            var subjects = db.Subjects.Where(s => records.FirstOrDefault(r => r.SubjectId == s.SubjectId) != null);
            var students = db.Students.Where(s => records.FirstOrDefault(r => r.StudentAccountId == s.AccountId) != null);
            var groups = students    
                .Select(s => s.StudentGroup)
                .Distinct()
                .OrderBy(s => s);

            TeacherAttendanceViewModel model = new TeacherAttendanceViewModel
            {
                Subjects = subjects,
                Groups = groups,
                Records = null,
                Students = null,

                CurrentSubject = null
            };
            
            if (gr != null && subj != null)
            {
                var newRecords = records.Where(r => r.SubjectId == subj && students.FirstOrDefault(s => s.AccountId == r.StudentAccountId).StudentGroup == gr);
                var newStudents = students.Where(s => newRecords.FirstOrDefault(r => r.StudentAccountId == s.AccountId) != null);
                var subject = db.Subjects.FirstOrDefault(s => s.SubjectId == subj);

                model.Records = newRecords;
                model.Students = newStudents;
                model.CurrentSubject = subject;
            }
            
            return View("Attendance", model);
        }
        [HttpPost]
        [Route("Attendance")]
        public IActionResult Attendance(TeacherAttendanceViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Attendance");
            return RedirectToAction("Attendance", new Dictionary<string, object> {
                { "gr", model.CurrentGroupId },
                { "subj", model.CurrentSubjectId}
            });
        }

        [HttpGet]
        [Route("AttendanceStudent")]
        public IActionResult AttendanceStudent(decimal? stud, decimal? subj)
        {
            if (stud != null && subj != null)
            {
                var student = db.Students.FirstOrDefault(s => s.AccountId == stud);
                var subject = db.Subjects.FirstOrDefault(s => s.SubjectId == subj);
                var teacher = CurrentUser;
                var journals = db.Journals.Where(j =>
                    j.StudentAccountId == student.AccountId &&
                    j.SubjectId == subject.SubjectId &&
                    j.TeacherAccountId == teacher.AccountId
                );
                StudentAttendanceViewModel model = new StudentAttendanceViewModel
                {
                    CurrentStudent = student,
                    CurrentSubject = subject,
                    Journals = journals
                };
                return View("AttendanceStudent", model);
            }
            else
                return RedirectToAction("Attendance");
        }
    }
}