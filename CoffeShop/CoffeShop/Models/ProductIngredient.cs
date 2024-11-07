using System;
using System.Collections.Generic;

namespace CoffeShop.Models
{
    public partial class ProductIngredient
    {
        public int IngredientId { get; set; }
        public int? MenuId { get; set; }
        public int? ItemId { get; set; }
        public int? QuantityPerProduct { get; set; }

        public virtual Inventory? Item { get; set; }
        public virtual Menu? Menu { get; set; }
    }
}
