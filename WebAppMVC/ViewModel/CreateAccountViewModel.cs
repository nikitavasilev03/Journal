using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.ViewModel
{
    public class CreateAccountViewModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateEnd { get; set; }
    }
}
