using System;
using System.Collections.Generic;

namespace CoffeShop.Models
{
    public partial class ProductPointRedemption
    {
        public int MenuId { get; set; }
        public int? RequiredPoints { get; set; }

        public virtual Menu Menu { get; set; } = null!;
    }
}
