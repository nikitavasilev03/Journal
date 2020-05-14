using DomainCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppMVC.ViewModel
{
    public class TeacherSubjectsViewModel
    {
        [Required]
        public decimal CurrentSubjectId { get; set; }

        public Subject CurrentSubject { get; set; }
        public Teacher CurrentTeacher{ get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public IEnumerable<Record> Records  { get; set; }
        public IEnumerable<Timetable> Timetable  { get; set; }
        public IEnumerable<Student> Students { get; set; }
        public string GetWeekDayByNumber(int day)
        {
            switch (day)
            {
                case 1:
                    return "Понедельник";
                case 2:
                    return "Вторник";
                case 3:
                    return "Среда";
                case 4:
                    return "Четверг";
                case 5:
                    return "Пятница";
                case 6:
                    return "Суббота";
                case 7:
                    return "Воскресение";
                default:
                    break;
            }
            return "";
        }
    }
}
