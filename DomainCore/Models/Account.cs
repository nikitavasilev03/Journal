using System;

namespace DomainCore.Models
{
    public partial class Account
    {
        public decimal AccountId { get; set; }
        public string LoginName { get; set; }
        public string Hpassword { get; set; }
        public string AccountType { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateEnd { get; set; }

        public virtual Student Students { get; set; }
        public virtual Teacher Teachers { get; set; }
    }
}
