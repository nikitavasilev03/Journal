using System;
using System.Collections.Generic;

namespace DomainCore.Models
{
    public partial class Journal
    {
        public decimal JourId { get; set; }
        public decimal SubjectId { get; set; }
        public decimal TeacherAccountId { get; set; }
        public decimal StudentAccountId { get; set; }
        public DateTime? VisitDate { get; set; }

        public virtual Student StudentAccount { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Teacher TeacherAccount { get; set; }
    }
}
