using System;
using System.Collections.Generic;

namespace E_Commerce_API.Models
{
    public partial class Users
    {
        public Users()
        {
            Customers = new HashSet<Customers>();
            Employees = new HashSet<Employees>();
            LogSingleton = new HashSet<LogSingleton>();
            Sale = new HashSet<Sale>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Customers> Customers { get; set; }
        public virtual ICollection<Employees> Employees { get; set; }
        public virtual ICollection<LogSingleton> LogSingleton { get; set; }
        public virtual ICollection<Sale> Sale { get; set; }
    }
}
