using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

using DomainCore.Models;
using System.Collections.Generic;

namespace WebAppMVC.ViewModel
{
    public class TeacherViewModel
    {
        [Required(ErrorMessage = "Не указан индификатор")]
        [Remote(action: "VerifyId", controller: "Admin")]
        public decimal Id { get; set; }

        [Required(ErrorMessage = "Не указана фамилия")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Не указано имя")]
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public Dictionary<string, int> Dependences { get; set; } = null;
        public TeacherViewModel()
        {

        }

        public TeacherViewModel(Teacher teacher)
        {
            Id = teacher.AccountId;
            SecondName = teacher.TeacherSname;
            FirstName = teacher.TeacherName;
            LastName = teacher.TeacherLname;
        }
    }
}
