
using CoffeShop.Filter;
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
		private readonly IngredientService ingredientService;
		private readonly InventoryService inventoryService;
		private readonly EmailService emailService;

		public ViewCartModel(CartService cartServicem, TableService tableService,
			UserService userService, OrderService orderService, IngredientService ingredientService,
			InventoryService inventoryService, EmailService emailService)
		{
			this.cartService = cartServicem;
			this.tableService = tableService;
			this.userService = userService;
			this.orderService = orderService;
			this.ingredientService = ingredientService;
			this.inventoryService = inventoryService;
			this.emailService = emailService;
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

		[BindProperty]
		public bool UseLoyaltyPoints { get; set; }
		[BindProperty]
		public int? PointsToRedeem { get; set; }
		[BindProperty]
		public DateTime ServiceDateTime { get; set; }



		public IActionResult OnGet()
		{
			LoadInforCartPage();

			return Page();
		}

		public IActionResult OnPostCheckOut()
		{
			var userId = HttpContext.Session.GetInt32("UserId");

			if(userId == null)
			{
				return RedirectToPage("/CoffeApp/Login");
			}

			LoadInforCartPage();
			var table = tableService.GetTable(TableNumber);

			if (table == null || table.Status != "Available")
			{
				ErrorMessage = "Cannot order this table. Please try again!";
				return Page();
			}

			if (ServiceDateTime < DateTime.Now)
			{
				ErrorMessage = "Service date and time cannot be in the past.";
				return Page();
			}

			//if (ServiceDateTime.Hour < 6 || ServiceDateTime.Hour >= 22)
			//{
			//	ErrorMessage = "Service time must be between 6 AM and 10 PM.";
			//	return Page();
			//}


			var cart = cartService.FindAllCartByUserId(userId);
			if (cart == null || cart.Count == 0)
			{
				ErrorMessage = "Your cart is empty. Please add items to your cart.";
				return Page();
			}

			if (!ValidateInventoryForCart(cart))
			{
				return Page();
			}

			decimal discount = 0;
			if (UseLoyaltyPoints && PointsToRedeem.HasValue)
			{
				var user = userService.FindUserById(userId);

				if (user.Poins < PointsToRedeem.Value)
				{
					ErrorMessage = "You do not have enough loyalty points.";
					return Page();
				}

				discount = PointsToRedeem.Value * 0.2m;

				if (discount > TotalCart)
				{
					discount = TotalCart;
					PointsToRedeem = (int)(TotalCart / 0.2m);
				}


				user.Poins -= PointsToRedeem.Value;
				user.PoinsUsed += PointsToRedeem.Value;
				userService.UpdateUser(user);
			}

			TotalCart -= discount;

			var order = new Order
			{
				UserId = userId,
				TableId = TableNumber,
				Status = "Confirmed",
				TotalPrice = TotalCart,
				CreatedAt = DateTime.Now,
				PointsUsed = PointsToRedeem ?? 0,
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
				if (!UpdateInventoryBasedOnOrder(cart))
				{
					return Page();
				}
				table.Status = "Reserved";
				table.ReservationTime = ServiceDateTime;
				tableService.UpdateTable(table);

				userService.AddLoyaltyPoints(userId, 1);
				SuccessMessage = "Order placed successfully!";

				var user = userService.FindUserById(userId);
				Task.Run(() => emailService.SendOrderConfirmationEmail(user.Email, user.FullName, order));

				CheckInventoryAndNotifyStaff(cart, table.Waiter);


				return Page();
			}
			else
			{
				ErrorMessage = "Order placement failed.";
				return Page();
			}
		}


		private void LoadInforCartPage()
		{
			int? userId = HttpContext.Session.GetInt32("UserId");

			Carts = cartService.FindAllCartByUserId(userId);

			TotalCart = Carts.Sum(cart => cart.PriceAtAdd);

			Tables = tableService.FindAllTable();

			if (userId != null)
			{
				User = userService.FindUserById(userId);
			}
			else
			{
				User = null;
			}
			
		}
		private bool ValidateInventoryForCart(List<Cart> cart)
		{
			foreach (var cartItem in cart)
			{
				var ingredients = ingredientService.FindIngredientsByMenuId(cartItem.MenuId);

				foreach (var ingredient in ingredients)
				{
					decimal totalQuantityNeeded = (ingredient.QuantityPerProduct.GetValueOrDefault() * cartItem.Quantity) / 1000m;
					var inventoryItem = inventoryService.GetInventoryItem(ingredient.ItemId.GetValueOrDefault());

					if (inventoryItem == null || inventoryItem.Quantity < totalQuantityNeeded)
					{
						ErrorMessage = cartItem.Menu.Name + "not enough inventory to order!";
						return false;
					}
				}
			}
			return true;
		}


		private bool UpdateInventoryBasedOnOrder(List<Cart> cart)
		{
			foreach (var cartItem in cart)
			{
				var ingredients = ingredientService.FindIngredientsByMenuId(cartItem.MenuId);

				foreach (var ingredient in ingredients)
				{
					decimal totalQuantityNeeded = (ingredient.QuantityPerProduct.GetValueOrDefault() * cartItem.Quantity) / 1000m;

					var inventoryItem = inventoryService.GetInventoryItem(ingredient.ItemId.GetValueOrDefault());
					if (inventoryItem != null && inventoryItem.Quantity >= totalQuantityNeeded)
					{
						inventoryItem.Quantity -= totalQuantityNeeded;
						inventoryService.UpdateInventory(inventoryItem);
					}
					else
					{
						ErrorMessage = $"Not enough {inventoryItem?.Name} in inventory.";
						return false;
					}
				}
			}
			return true;
		}

		private void CheckInventoryAndNotifyStaff(List<Cart> cart, int? staffId)
		{
			List<string> lowInventoryItems = new List<string>();

			foreach (var cartItem in cart)
			{
				var ingredients = ingredientService.FindIngredientsByMenuId(cartItem.MenuId);

				foreach (var ingredient in ingredients)
				{
					var inventoryItem = inventoryService.GetInventoryItem(ingredient.ItemId.GetValueOrDefault());

					if (inventoryItem != null && inventoryItem.Quantity <= inventoryItem.MinimumQuantity.GetValueOrDefault())
					{
						lowInventoryItems.Add($"{inventoryItem.Name} (Required: {ingredient.QuantityPerProduct * cartItem.Quantity / 1000m} units)");
					}
				}
			}

			if (lowInventoryItems.Any())
			{
				var staff = userService.FindUserById(staffId);
				Task.Run(() => emailService.SendLowInventoryNotification(lowInventoryItems, staff.Email));
			}
		}

	}
}
