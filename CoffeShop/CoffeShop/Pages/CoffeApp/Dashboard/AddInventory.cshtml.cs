using CoffeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    public class AddInventoryModel : PageModel
    {
        [BindProperty]
        public Inventory InventoryItem { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (InventoryItem.MinimumQuantity >= InventoryItem.Quantity)
            {
                ModelState.AddModelError(string.Empty, "Minimum Quantity must be less than Quantity.");
                return Page();
            }

            CoffeShopContext.Ins.Inventories.Add(InventoryItem);
            await CoffeShopContext.Ins.SaveChangesAsync();

            return RedirectToPage("/CoffeApp/Dashboard/ManageInventory");
        }
    }
}
