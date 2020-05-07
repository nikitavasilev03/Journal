using System;
using System.ComponentModel.DataAnnotations;
using DomainCore.Models;

namespace WebAppMVC.ViewModel
{
    public class AccountViewModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
        
        //[DataType(DataType.Date)]
        //public DateTime? DateStart { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? DateEnd { get; set; }
        public AccountViewModel()
        {

        }

        public AccountViewModel(Account account)
        {
            Login = account.LoginName;
            DateEnd = account.DateEnd;
        }
    }
}
