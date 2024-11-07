using System;
using System.Collections.Generic;

namespace CoffeShop.Models
{
    public partial class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int MenuId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtAdd { get; set; }
        public DateTime? AddedAt { get; set; }

        public virtual Menu Menu { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
