using System;
using System.Collections.Generic;

namespace DomainCore.Models
{
    public partial class Timetable
    {
        public decimal TtId { get; set; }
        public decimal RecordId { get; set; }
        public decimal TeacherAccountId { get; set; }
        public decimal? TtWeekDay { get; set; }
        public decimal? TtNumLesson { get; set; }

        public virtual Record Record { get; set; }
        public virtual Teacher TeacherAccount { get; set; }
    }
}
