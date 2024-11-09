using System;
using System.Collections.Generic;

namespace CoffeShop.Models
{
    public partial class LoyaltyPoint
    {
        public int UserId { get; set; }
        public int? Points { get; set; }
        public int? PointsUsed { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
