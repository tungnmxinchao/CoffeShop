using System;
using System.Collections.Generic;

namespace CoffeShop.Models
{
    public partial class Menu
    {
        public Menu()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
            ProductIngredients = new HashSet<ProductIngredient>();
        }

        public int MenuId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public string? ImageUrl { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductIngredient> ProductIngredients { get; set; }
    }
}
