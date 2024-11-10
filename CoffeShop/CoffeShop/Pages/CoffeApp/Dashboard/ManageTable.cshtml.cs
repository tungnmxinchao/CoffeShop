using CoffeShop.Models;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    public class ManageTableModel : PageModel
    {
        private readonly TableService tableService;

		public ManageTableModel(TableService tableService)
		{
			this.tableService = tableService;
		}

        public List<Table> Tables { get; set; }

		public IActionResult OnGet()
        {
			Tables = tableService.FindAllTable();

			return Page();
        }
    }
}
