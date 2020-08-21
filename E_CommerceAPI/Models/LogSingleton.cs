using System;
using System.Collections.Generic;

namespace E_Commerce_API.Models
{
    public partial class LogSingleton
    {
        public int LogId { get; set; }
        public string LogType { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime LogDate { get; set; }

        public virtual Product Product { get; set; }
        public virtual Users User { get; set; }
    }
}
