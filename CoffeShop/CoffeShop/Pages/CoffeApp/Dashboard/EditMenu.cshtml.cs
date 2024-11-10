using CoffeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    public class EditMenuModel : PageModel
    {
        private readonly CoffeShopContext _context;
        public EditMenuModel(CoffeShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Menu Product { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        public List<Category> Categories { get; set; }

        [BindProperty]
        public int SelectedCategoryId { get; set; }
        [BindProperty]
        public Menu ProductFind { get; set; }
        [BindProperty]
        public int ProductID { get; set; }

        public List<Inventory> ListInventory { get; set; }

        [BindProperty]
        public List<int> SelectedIngredients { get; set; } = new List<int>();

        [BindProperty]
        public Dictionary<int, int> QuantityPerProduct { get; set; } = new Dictionary<int, int>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ProductID = id;


            ProductFind = await _context.Menus
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.MenuId == id);


            Categories = await _context.Categories.ToListAsync();


            ListInventory = await _context.Inventories.ToListAsync();


            var existingIngredients = await _context.ProductIngredients
                .Where(pi => pi.MenuId == id)
                .ToListAsync();


            SelectedIngredients = existingIngredients.Select(pi => pi.ItemId ?? 0).ToList();
            QuantityPerProduct = existingIngredients.ToDictionary(pi => pi.ItemId ?? 0, pi => pi.QuantityPerProduct ?? 0);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using (CoffeShopContext context = new CoffeShopContext())
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



                var productToUpdate = await context.Menus.FirstOrDefaultAsync(x => x.MenuId == ProductID);
                if (productToUpdate == null) return NotFound();


                productToUpdate.Name = Product.Name;
                productToUpdate.Description = Product.Description;
                productToUpdate.Price = Product.Price;
                productToUpdate.CategoryId = SelectedCategoryId;
                productToUpdate.ImageUrl = relativeImagePath;


                var existingIngredients = await context.ProductIngredients.Where(pi => pi.MenuId == ProductID).ToListAsync();
                context.ProductIngredients.RemoveRange(existingIngredients);

                foreach (var ingredientId in SelectedIngredients)
                {
                    if (QuantityPerProduct.TryGetValue(ingredientId, out int quantity) && quantity > 0)
                    {
                        var productIngredient = new ProductIngredient
                        {
                            MenuId = ProductID,
                            ItemId = ingredientId,
                            QuantityPerProduct = quantity
                        };
                        context.ProductIngredients.Add(productIngredient);
                    }
                }

                await context.SaveChangesAsync();

                return RedirectToPage("/CoffeApp/Dashboard/ManageMenu");
            }
        }







    }
}
