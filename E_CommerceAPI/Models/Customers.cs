using System;
using System.Collections.Generic;

namespace E_Commerce_API.Models
{
    public partial class Customers
    {
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public int StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string Suburb { get; set; }
        public string Province { get; set; }

        public virtual Users User { get; set; }
    }
}
