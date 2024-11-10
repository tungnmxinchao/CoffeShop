using CoffeShop.Filter;
using CoffeShop.Models;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    [RequireManager]
    public class ManageOrderModel : PageModel
    {

        private readonly OrderService orderService;

		public ManageOrderModel(OrderService orderService)
		{
			this.orderService = orderService;
		}

		public List<Order> Orders { get; set; }

		public IActionResult OnGet()
        {
			Orders = orderService.FindAllOrders();

			return Page();
        }
    }
}
