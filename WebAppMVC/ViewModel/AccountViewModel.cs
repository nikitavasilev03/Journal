using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DomainCore.Models;

namespace WebAppMVC.ViewModel
{
    public class AccountViewModel
    {
        [Required(ErrorMessage = "Не указан индификатор")]
        public decimal Id { get; set; }
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }
        
        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Не указан тип учетной записи")]
        public string AccountType { get; set; }   
        
        [DataType(DataType.Date)]
        public DateTime? DateEnd { get; set; }
        
        public bool IsTermless { get; set; }
        public bool IsChangePassword { get; set; }

        public Dictionary<string, int> Dependences { get; set; } = null;

        public AccountViewModel()
        {

        }
        
        public AccountViewModel(Account account)
        {
            Id = account.AccountId;
            Login = account.LoginName;
            DateEnd = account.DateEnd;
            AccountType = account.AccountType;
        }
    }
}
