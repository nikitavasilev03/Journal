using System;
using System.Collections.Generic;

namespace DomainCore.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            Journals = new HashSet<Journal>();
            Timetable = new HashSet<Timetable>();
        }

        public decimal AccountId { get; set; }
        public string TeacherName { get; set; }
        public string TeacherSname { get; set; }
        public string TeacherLname { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<Journal> Journals { get; set; }
        public virtual ICollection<Timetable> Timetable { get; set; }
    }
}
