using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

using DomainCore.Models;
using System.Collections.Generic;

namespace WebAppMVC.ViewModel
{
    public class StudentViewModel
    {
        [Required(ErrorMessage = "Не указан индификатор")]
        [Remote(action: "VerifyId", controller: "Admin")]
        public decimal Id { get; set; }

        [Required(ErrorMessage = "Не указана фамилия")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Не указано имя")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Не указан группа")]
        public decimal Group { get; set; }

        public Dictionary<string, int> Dependences { get; set; } = null;

        public StudentViewModel()
        {

        }

        public StudentViewModel(Student student)
        {
            Id = student.AccountId;
            SecondName = student.StudentSname;
            FirstName = student.StudentName;
            LastName = student.StudentLname;
            Group = student.StudentGroup;
        }
    }
}
