using CoffeShop.Filter;
using CoffeShop.Models;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    [RequireManager]
    public class TableServiceProcessModel : PageModel
    {
		private readonly TableService tableService;
		private readonly OrderService orderService;

		public TableServiceProcessModel(TableService tableService, OrderService orderService)
		{
			this.tableService = tableService;
			this.orderService = orderService;
		}

		public List<Table> Tables { get; set; }

		public Dictionary<int, List<Order>> ReservedOrders { get; set; }

		[BindProperty]
		public int TableId { get; set; }

		[BindProperty]
		public string TableStatus {  get; set; }

		public IActionResult OnGet()
		{
			Tables = tableService.FindAllTable();
			ReservedOrders = new Dictionary<int, List<Order>>();

			foreach (var table in Tables)
			{
				if (table.Status == "Reserved" || table.Status == "Occupied")
				{
					var latestOrder = orderService.GetLatestOrderByTableId(table.TableId);
					if (latestOrder != null)
					{
						if (!ReservedOrders.ContainsKey(table.TableId))
						{
							ReservedOrders[table.TableId] = new List<Order>();
						}
						ReservedOrders[table.TableId].Add(latestOrder);
					}
				}
			}

			return Page();
		}

		public IActionResult OnPost()
		{

			if (TableId > 0 && !string.IsNullOrEmpty(TableStatus))
			{
				var table = tableService.GetTable(TableId);
				if (table != null)
				{
					table.Status = TableStatus;
					tableService.UpdateTable(table);
				}
			}

			return RedirectToPage();
		}
	}
}
