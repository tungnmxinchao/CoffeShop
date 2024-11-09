
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

		public ViewCartModel(CartService cartServicem, TableService tableService,
			UserService userService, OrderService orderService, IngredientService ingredientService,
			InventoryService inventoryService)
		{
			this.cartService = cartServicem;
			this.tableService = tableService;
			this.userService = userService;
			this.orderService = orderService;
			this.ingredientService = ingredientService;
			this.inventoryService = inventoryService;
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
		public DateTime ServiceDateTime {  get; set; }



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

			if (!ValidateInventoryForCart(cart))
			{
				return Page();
			}

			decimal discount = 0;
			if (UseLoyaltyPoints && PointsToRedeem.HasValue)
			{
				var user = userService.FindUserById(1);

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
				UserId = 1,
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
				tableService.UpdateTable(table);

				userService.AddLoyaltyPoints(1, 1);
				SuccessMessage = "Order placed successfully!";
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
			Carts = cartService.FindAllCartByUserId(1);

			TotalCart = Carts.Sum(cart => cart.PriceAtAdd);

			Tables = tableService.FindAllTable();

			User = userService.FindUserById(1);
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

	}
}
