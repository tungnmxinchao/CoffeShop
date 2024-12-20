using CoffeShop.Models;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    public class ViewOrderDetailsModel : PageModel
    {
        private readonly OrderService orderService;
        private readonly OrderDetailsService orderDetailsService;
        private readonly EmailService emailService;
        private readonly UserService userService;
        private readonly InventoryService inventoryService;
        private readonly IngredientService ingredientService;

		public ViewOrderDetailsModel(OrderService orderService, OrderDetailsService orderDetailsService, 
            EmailService emailService, UserService userService , InventoryService inventoryService,
            IngredientService ingredientService)
		{
			this.orderService = orderService;
			this.orderDetailsService = orderDetailsService;
			this.emailService = emailService;
            this.userService = userService;
            this.inventoryService = inventoryService;
            this.ingredientService = ingredientService;
		}

		[BindProperty(SupportsGet = true)]
        public int OrderId { get; set; }

        public Order Order { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

		[BindProperty]
		public string Status {  get; set; }
		[BindProperty]
		public string Reason { get; set; }

        [BindProperty]
        public int OrderIdUpdate {  get; set; }
        public IActionResult OnGet()
        {

            LoadPage();
			return Page();

        }

		public IActionResult OnPostUpdateStatus()
		{
			if (Status == "Cancelled")
			{
				var order = orderService.FindOrderById(OrderIdUpdate);

				if (order.Status != "Cancelled")
				{
					order.Status = Status;

					var user = userService.FindUserById(order.UserId);


					foreach (var orderDetail in order.OrderDetails)
					{
						var menuId = orderDetail.MenuId;
						var quantityOrdered = orderDetail.Quantity ?? 0;

						var productIngredients = ingredientService.FindIngredientsByMenuId(menuId);

						foreach (var productIngredient in productIngredients)
						{
				
							var inventoryItem = inventoryService.GetInventoryItem(productIngredient.ItemId);

							if (inventoryItem != null)
							{
								inventoryItem.Quantity += (productIngredient.QuantityPerProduct * quantityOrdered ) / 1000m;

								inventoryService.UpdateInventory(inventoryItem);
							}
						}
					}
					orderService.UpdateOrder(order);

					Task.Run(() => emailService.SendCancelOrder(Reason, user.Email, user.FullName, order));
				}
			}
			else
			{
				var order = orderService.FindOrderById(OrderIdUpdate);
				order.Status = Status;
				orderService.UpdateOrder(order);
			}

			OrderId = OrderIdUpdate;
			LoadPage();

			return Page();
		}

		private void LoadPage()
        {
			Order = orderService.FindOrderById(OrderId);
			OrderDetails = orderDetailsService.FindByOrderId(OrderId);
		}

	}
}
