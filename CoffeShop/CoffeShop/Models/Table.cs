using System;
using System.Collections.Generic;

namespace CoffeShop.Models
{
    public partial class Table
    {
        public Table()
        {
            Feedbacks = new HashSet<Feedback>();
            Orders = new HashSet<Order>();
        }

        public int TableId { get; set; }
        public string? Status { get; set; }
        public int? Capacity { get; set; }
        public string? Location { get; set; }
        public DateTime? ReservationTime { get; set; }
        public int? Waiter { get; set; }

        public virtual User? WaiterNavigation { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
