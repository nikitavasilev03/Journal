using DomainCore.Models;
using System.Collections.Generic;

namespace WebAppMVC.ViewModel
{
    public class StudentsSubjectsViewModel
    {
        public Student Student { get; set; }
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public IEnumerable<Record> Records { get; set; }
    }
}
