using DomainCore;

using System;
using System.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var appContext = DBProvider.Context)
            {
                var accounts = appContext.Accounts.ToList();
                foreach (var item in accounts)
                {
                    Console.WriteLine($"{item.AccountId} {item.LoginName}");
                }
                var students = appContext.Students.ToList();
                
                foreach (var item in students)
                {
                    Console.WriteLine($"{item.StudentName} {item.Account.AccountId}");
                }
            }
        }
    }
}
