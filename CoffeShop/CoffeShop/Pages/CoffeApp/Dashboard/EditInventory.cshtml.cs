using CoffeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    public class EditInventoryModel : PageModel
    {
        [BindProperty]
        public Inventory InventoryFind { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			InventoryFind = CoffeShopContext.Ins.Inventories.FirstOrDefault(x => x.ItemId == id);


			return Page();
		}


        public async Task<IActionResult> OnPostAsync()
        {
          

            var inventoryToUpdate = await CoffeShopContext.Ins.Inventories.FirstOrDefaultAsync(x => x.ItemId == InventoryFind.ItemId);
            if (InventoryFind.Quantity <= InventoryFind.MinimumQuantity)
            {
                ModelState.AddModelError(string.Empty, "Quantity must be greater than Minimum Quantity.");
                return Page();
            }
            else
            {
                inventoryToUpdate.Name = InventoryFind.Name;
                inventoryToUpdate.Quantity = InventoryFind.Quantity;
                inventoryToUpdate.LastRestocked = InventoryFind.LastRestocked;
                inventoryToUpdate.Price = InventoryFind.Price;
                inventoryToUpdate.MinimumQuantity = InventoryFind.MinimumQuantity;

                await CoffeShopContext.Ins.SaveChangesAsync();

                return RedirectToPage("/CoffeApp/Dashboard/ManageInventory");
            }
        }
    }
}

