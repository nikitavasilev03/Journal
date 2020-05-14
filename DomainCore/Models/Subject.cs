using System;
using System.Collections.Generic;

namespace DomainCore.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Journals = new HashSet<Journal>();
            Records = new HashSet<Record>();
        }

        public decimal SubjectId { get; set; }
        public string SubjectName { get; set; }
        public decimal? NeedVisits { get; set; }

        public virtual ICollection<Journal> Journals { get; set; }
        public virtual ICollection<Record> Records { get; set; }
    }
}
