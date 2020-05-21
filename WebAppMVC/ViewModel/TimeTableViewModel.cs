using DomainCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace WebAppMVC.ViewModel
{
    public class TimeTableViewModel
    {
        public IEnumerable<Timetable> Timetable { get; set; }
        public IEnumerable<Record> Records { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }
        public int DayOfWeek { get; set; } = 1;

        public string GetNameSubjectByID(decimal id)
        {
            return Subjects.FirstOrDefault(s => s.SubjectId == id).SubjectName;
        }
        public string GetWeekDayByNumber()
        {
            switch (DayOfWeek)
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
