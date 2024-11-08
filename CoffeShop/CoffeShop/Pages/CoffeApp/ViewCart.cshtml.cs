
using CoffeShop.Models;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp
{
	public class ViewCartModel : PageModel
	{

		private readonly CartService cartService;
		private readonly TableService tableService;
		private readonly UserService userService;
		private readonly OrderService orderService;

		public ViewCartModel(CartService cartServicem, TableService tableService,
			UserService userService, OrderService orderService)
		{
			this.cartService = cartServicem;
			this.tableService = tableService;
			this.userService = userService;
			this.orderService = orderService;
		}

		public List<Cart> Carts { get; set; }

		[BindProperty]
		public decimal TotalCart { get; set; }

		public List<Table> Tables { get; set; }

		[BindProperty]
		public User User { get; set; }

		[BindProperty]
		public int TableNumber { get; set; }

		public string ErrorMessage { get; set; }

		public string SuccessMessage { get; set; }



		public IActionResult OnGet()
		{
			LoadInforCartPage();

			return Page();
		}

		public IActionResult OnPostCheckOut()
		{
			LoadInforCartPage();
			var table = tableService.GetTable(TableNumber);

			if (table == null || table.Status != "Available")
			{
				ErrorMessage = "Cannot order this table. Please try again!";
				return Page();
			}

			var cart = cartService.FindAllCartByUserId(1);

			if (cart == null || cart.Count == 0)
			{
				ErrorMessage = "Your cart is empty. Please add items to your cart.";
				return Page();
			}

			var order = new Order
			{
				UserId = 1,
				TableId = TableNumber,
				Status = "Pending",
				TotalPrice = TotalCart,
				CreatedAt = DateTime.Now
			};

			foreach (var cartItem in cart)
			{
				var orderDetail = new OrderDetail
				{
					MenuId = cartItem.MenuId,
					Quantity = cartItem.Quantity,
					Price = cartItem.PriceAtAdd
				};

				order.OrderDetails.Add(orderDetail);
			}

			if (orderService.CreateOrder(order))
			{
				SuccessMessage = "Order Successfully!";
				return Page();

			}
			else
			{
				return Page();
			}
		}


		private void LoadInforCartPage()
		{
			Carts = cartService.FindAllCartByUserId(1);

			TotalCart = Carts.Sum(cart => cart.PriceAtAdd);

			Tables = tableService.FindAllTable();

			User = userService.FindUserById(1);
		}

	}
}
