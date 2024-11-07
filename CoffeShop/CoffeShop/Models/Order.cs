using System;
using System.Collections.Generic;

namespace CoffeShop.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public int? TableId { get; set; }
        public string? Status { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Table? Table { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
