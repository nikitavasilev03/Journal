using DomainCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebAppMVC.ViewModel
{
    public class JournalViewModel
    {
        [Required]
        public decimal CurrentSubjectId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Day { get; set; }
        [Required]
        public int NumberLeson { get; set; } = 1;

        public Subject CurrentSubject { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public IEnumerable<Journal> Journals { get; set; }
        public IEnumerable<Student> Students { get; set; }

        public string GetNameSubjectByID(decimal id)
        {
            return Subjects.FirstOrDefault(s => s.SubjectId == id).SubjectName;
        }
    }
}
