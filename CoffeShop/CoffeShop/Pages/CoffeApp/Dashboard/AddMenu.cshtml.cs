using CoffeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
	public class AddMenuModel : PageModel
	{
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public decimal Price { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        [BindProperty]
        public int CategoryId { get; set; }

        public List<Category> Categories { get; set; }

        private readonly CoffeShopContext _context;

        public List<Inventory> ListInventory { get; set; }

        [BindProperty]
        public List<int> SelectedIngredients { get; set; } = new List<int>();

        [BindProperty]
        public Dictionary<int, int> QuantityPerProduct { get; set; } = new Dictionary<int, int>();

        public ProductIngredient p { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Categories = await CoffeShopContext.Ins.Categories.ToListAsync();
            ListInventory = CoffeShopContext.Ins.Inventories.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
                string fileName = Path.GetFileName(Image.FileName);
                string folderPath = Path.Combine("D:", "Semester 7", "ASM_PRN", "Final PRN211", "Final PRN211", "CoffeShop", "CoffeShop", "CoffeShop", "wwwroot", "Image");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }

                string relativeImagePath = "/Image/" + fileName;

                using (CoffeShopContext context = new CoffeShopContext())
            {
                var menuItem = new Menu
                {
                    Name = Name,
                    Price = Price,
                    Description = Description,
                    CategoryId = CategoryId,
                    ImageUrl = relativeImagePath
                };

                context.Menus.Add(menuItem);
                await context.SaveChangesAsync();

                foreach (var ingredientId in SelectedIngredients)
                {
                    if (QuantityPerProduct.TryGetValue(ingredientId, out int quantity) && quantity > 0)
                    {
                        p = new ProductIngredient
                        {
                            MenuId = menuItem.MenuId,
                            ItemId = ingredientId,
                            QuantityPerProduct = quantity
                        };

                        context.ProductIngredients.Add(p);
                    }
                }

          
                await context.SaveChangesAsync();
               
                return RedirectToPage("/CoffeApp/Dashboard/ManageMenu");
            }
        }

    }
}
