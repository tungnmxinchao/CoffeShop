using CoffeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    public class ManageInventoryModel : PageModel
    {
		public List<Inventory> ListInventory { get; set; }
		public void OnGet()
        {
			ListInventory = CoffeShopContext.Ins.Inventories.ToList();

		}
    }
}
