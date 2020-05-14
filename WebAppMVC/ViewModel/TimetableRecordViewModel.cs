using DomainCore.Models;

namespace WebAppMVC.ViewModel
{
    public class TimetableRecordViewModel
    {
        public decimal Id { get; set; }
        public decimal TeacherId { get; set; }
        public decimal SubjectId { get; set; }
        public decimal RecordId { get; set; }
        public decimal? DayOfWeek { get; set; }
        public decimal? NumberLesson { get; set; }
        public TimetableRecordViewModel()
        {

        }
        public TimetableRecordViewModel(Timetable timetable)
        {
            Id = timetable.TtId;
            TeacherId = timetable.TeacherAccountId;
            RecordId = timetable.RecordId;
            DayOfWeek = timetable.TtWeekDay;
            NumberLesson = timetable.TtNumLesson;
        }
    }
}
