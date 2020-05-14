using DomainCore.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebAppMVC.ViewModel
{
    public class TeacherAttendanceViewModel
    {
        [Required(ErrorMessage = "Не указана группа")]
        public decimal CurrentGroupId { get; set; }
        [Required(ErrorMessage = "Не указан предмет")]
        public decimal CurrentSubjectId { get; set; }

        public Subject CurrentSubject { get; set; }

        public IQueryable<Record> Records { get; set; }
        public IQueryable<Student> Students { get; set; }
        public IQueryable<Subject> Subjects { get; set; }
        public IQueryable<decimal> Groups { get; set; }
    }
}
