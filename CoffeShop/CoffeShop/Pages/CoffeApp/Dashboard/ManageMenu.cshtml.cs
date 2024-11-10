using CoffeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    public class ManageMenuModel : PageModel
    {
        public List<Menu> Products { get; set; }
        public List<Category> Category { get; set; }

        [BindProperty]
        public int SelectedCategoryId { get; set; }
        public void OnGet()
        {
            using (CoffeShopContext context = new CoffeShopContext())
            {
                Products = context.Menus.Include(x => x.Category).ToList();

                Category = context.Categories.ToList();

                Category.Insert(0, new Category { CategoryId = 0, Name = "All Categories" });
            }

        }

        public void OnPost()
        {
            using (CoffeShopContext context = new CoffeShopContext())
            {
                var selectedCategoryId = SelectedCategoryId;

                if (selectedCategoryId == 0)
                {
                    Products = context.Menus.Include(x => x.Category).ToList();

                }
                else
                {
                    Products = context.Menus.Include(x => x.Category)
                    .Where(x => x.CategoryId == selectedCategoryId)
                    .ToList();
                }


                Category = context.Categories.ToList();
                Category.Insert(0, new Category { CategoryId = 0, Name = "All Categories" });
            }
        }

    }

}
