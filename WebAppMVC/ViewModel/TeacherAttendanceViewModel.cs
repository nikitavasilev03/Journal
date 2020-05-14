using DomainCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppMVC.ViewModel
{
    public class TeacherAttendanceViewModel
    {
        public decimal CurrentGroupId { get; set; }
        public decimal CurrentSubjectId { get; set; }

        public Subject CurrentSubject { get; set; }

        public IQueryable<Record> Records { get; set; }
        public IQueryable<Student> Students { get; set; }
        public IQueryable<Subject> Subjects { get; set; }
        public IQueryable<decimal> Groups { get; set; }
    }
}
