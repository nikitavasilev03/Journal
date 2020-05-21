using DomainCore.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebAppMVC.ViewModel
{
    public class StudentAttendanceViewModel
    {
        [Required(ErrorMessage = "Не указан предмет")]
        public decimal CurrentSubjectId { get; set; }
        public Student CurrentStudent { get; set; }
        public Subject CurrentSubject { get; set; }

        public IQueryable<Subject> Subjects { get; set; }
        public IQueryable<Record> Records { get; set; }
        public IQueryable<Journal> Journals { get; set; }
        public IQueryable<Teacher> Teachers { get; set; }

        public string GetWeekDayByNumber(int number)
        {
            switch (number)
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
