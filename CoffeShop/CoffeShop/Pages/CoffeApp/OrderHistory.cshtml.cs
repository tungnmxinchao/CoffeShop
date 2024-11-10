using CoffeShop.Models;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp
{
    public class OrderHistoryModel : PageModel
    {

        private readonly OrderService orderService;
		private readonly OrderDetailsService orderDetailsService;

		public OrderHistoryModel(OrderService orderService, OrderDetailsService orderDetailsService)
		{
			this.orderService = orderService;
			this.orderDetailsService = orderDetailsService;
		}

        public List<Order> Orders {  get; set; }

		[BindProperty]
		public int OrderId { get; set; }

		public List<OrderDetail> OrderDetails { get; set; }

		public IActionResult OnGet()
        {

			LoadOrders();
			return Page();


		}

		public IActionResult OnPostDetailsOrder() {
			if(OrderId != 0)
			{
				OrderDetails = orderDetailsService.FindByOrderId(OrderId);
			}
			LoadOrders();

			return Page();
		}

		private void LoadOrders()
		{
			var userId = HttpContext.Session.GetInt32("UserId");

			Orders = orderService.FindAllOrdersByUserId(userId);
		}
    }
}
