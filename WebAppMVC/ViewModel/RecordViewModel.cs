using DomainCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.ViewModel
{
    public class RecordViewModel
    {
        public decimal Id { get; set; }
        [Required(ErrorMessage = "Не указана дата")]
        [DataType(DataType.Date)]
        public DateTime DateEnd { get; set; }
        [Required(ErrorMessage = "Не указан предмет")]
        public decimal SubjectId { get; set; }

        public Student Student { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public Dictionary<string, int> Dependences { get; set; } = null;

        public RecordViewModel()
        {

        }

        public RecordViewModel(Record record)
        {
            Id = record.RecordId;
            DateEnd = record.DateEnd;
            SubjectId = record.SubjectId;
        }

    }
}
