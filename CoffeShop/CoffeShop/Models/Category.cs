using System;
using System.Collections.Generic;

namespace CoffeShop.Models
{
    public partial class Category
    {
        public Category()
        {
            Menus = new HashSet<Menu>();
        }

        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }
    }
}
