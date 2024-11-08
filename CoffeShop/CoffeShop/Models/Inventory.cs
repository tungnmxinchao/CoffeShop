using System;
using System.Collections.Generic;

namespace CoffeShop.Models
{
    public partial class Inventory
    {
        public Inventory()
        {
            ProductIngredients = new HashSet<ProductIngredient>();
        }

        public int ItemId { get; set; }
        public string? Name { get; set; }
        public decimal? Quantity { get; set; }
        public string? Unit { get; set; }
        public DateTime? LastRestocked { get; set; }
        public int? MinimumQuantity { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<ProductIngredient> ProductIngredients { get; set; }
    }
}
