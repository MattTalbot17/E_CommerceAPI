using System;
using System.Collections.Generic;

namespace E_Commerce_API.Models
{
    public partial class Sale
    {
        public int SaleId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateOfSale { get; set; }

        public virtual Product Product { get; set; }
        public virtual Users User { get; set; }
    }
}
