using DomainCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppMVC.ViewModel
{
    public class SubjectViewModel
    {
        public decimal Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указано количество посещений")]
        public decimal? NeedVisits { get; set; }

        public Dictionary<string, int> Dependences { get; set; } = null;

        public SubjectViewModel()
        {

        }

        public SubjectViewModel(Subject subject)
        {
            Id = subject.SubjectId;
            Name = subject.SubjectName;
            NeedVisits = subject.NeedVisits;
        }
    }
}
