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
        [Route("Timetable")]
        public async Task<IActionResult> Timetable()
        {
            var tt = CurrnetTimetable.AsEnumerable()
                .GroupBy(t => t.RecordId)
                .Select(t => t.First());
            foreach (var item in tt)
                item.Record = await db.Records.FirstOrDefaultAsync(r => r.RecordId == item.RecordId);

            TimeTableViewModel model = new TimeTableViewModel
            {
                Timetable = tt,
                Subjects = db.Subjects
            };

            return View("Timetable", model);
        }
        [Route("Attendance")]
        public IActionResult Attendance()
        {
            var subjects = db.Subjects.
                Where(s => CurrnetAttendance.FirstOrDefault(a => a.SubjectId == s.SubjectId) != null);

            AttendanceViewModel model = new AttendanceViewModel
            {
                Subjects = subjects,
                CurrentSubjectId = subjects.FirstOrDefault().SubjectId,
                Day = DateTime.Now
            };

            return View("Attendance", model);
        }
        [HttpPost]
        [Route("ShowJournal")]
        public IActionResult ShowJournal(AttendanceViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Attendance");

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

                return View("Attendance", model);
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

                return View("Attendance", model);
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

            return View("Attendance", model);
        }

        [HttpPost]
        [Route("AttendanceAdd")]
        public void AttendanceAdd(int subjectId, int studentId, DateTime date)
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
        [Route("AttendanceRemove")]
        public void AttendanceRemove(int subjectId, int studentId, DateTime date)
        {
            var journal = db.Journals.FirstOrDefault(
                j => j.StudentAccountId == subjectId &&
                     j.SubjectId == studentId &&
                     j.VisitDate == date
            );
            
            if (journal == null)
                return;

            db.Journals.Remove(journal);
            db.SaveChanges();
        }

        #endregion
    }
}