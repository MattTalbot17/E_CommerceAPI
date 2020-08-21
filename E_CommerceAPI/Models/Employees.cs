using System;
using System.Collections.Generic;

namespace E_Commerce_API.Models
{
    public partial class Employees
    {
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public string EmployeePosition { get; set; }

        public virtual Users User { get; set; }
    }
}
