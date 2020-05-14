using System;
using System.Collections.Generic;

namespace DomainCore.Models
{
    public partial class Student
    {
        public Student()
        {
            Journals = new HashSet<Journal>();
            Records = new HashSet<Record>();
        }

        public decimal AccountId { get; set; }
        public string StudentName { get; set; }
        public string StudentSname { get; set; }
        public string StudentLname { get; set; }
        public decimal StudentGroup { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<Journal> Journals { get; set; }
        public virtual ICollection<Record> Records { get; set; }
    }
}
