using System;
using System.Collections.Generic;

namespace DomainCore.Models
{
    public partial class Record
    {
        public Record()
        {
            Timetable = new HashSet<Timetable>();
        }

        public decimal RecordId { get; set; }
        public decimal StudentAccountId { get; set; }
        public decimal SubjectId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public decimal NumberVisits { get; set; }
        public bool IsPassed { get; set; }

        public virtual Student StudentAccount { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<Timetable> Timetable { get; set; }
    }
}
