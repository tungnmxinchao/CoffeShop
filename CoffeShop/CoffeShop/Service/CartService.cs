using CoffeShop.Models;

namespace CoffeShop.Service
{
	public class CartService
	{

		public List<Cart> FindAllCartByUserId(int userId)
		{
			return CoffeShopContext.Ins.Carts.Where(x => x.UserId == userId).ToList();
		}

		public bool AddToCart(int idMenu, int quantity)
		{
			var menu = CoffeShopContext.Ins.Menus.Find(idMenu);

			if (menu == null)
			{
				return false;
			}

			var menuInCart = CoffeShopContext.Ins.Carts.Find(idMenu);

			if (menuInCart == null)
			{
				Cart cart = new Cart
				{
					UserId = 1,
					MenuId = idMenu,
					Quantity = quantity,
					PriceAtAdd = (decimal)(menu.Price * quantity)
				};

				try
				{
					CoffeShopContext.Ins.Carts.Add(cart);
					CoffeShopContext.Ins.SaveChanges();

					return true;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
					return false;
				}
			}
			else
			{
				menuInCart.Quantity += quantity;
				menuInCart.PriceAtAdd = (decimal)(menu.Price * menuInCart.Quantity);

				try
				{
					CoffeShopContext.Ins.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
					return false;
				}
			}
		}

		public bool UpdateCart(List<Cart> carts, string[] newQuantities)
		{
			if (carts.Count != newQuantities.Length)
			{
				Console.WriteLine("Error: The number of cart items and quantities must match.");
				return false;
			}

			try
			{
				for (int i = 0; i < carts.Count; i++)
				{
					var cartItem = carts[i];
					int newQuantity;

					if (!int.TryParse(newQuantities[i], out newQuantity))
					{
						Console.WriteLine($"Error: Quantity '{newQuantities[i]}' is not a valid integer.");
						return false;
					}

					if (newQuantity <= 0)
					{
						CoffeShopContext.Ins.Carts.Remove(cartItem);
					}
					else
					{
						cartItem.Quantity = newQuantity;
						cartItem.PriceAtAdd = (decimal)(cartItem.Menu.Price * newQuantity);
					}
				}

				CoffeShopContext.Ins.SaveChanges();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public bool RemoveCart(int userId, int[] idMenus)
		{
			try
			{
				foreach (int idMenu in idMenus)
				{
					var menuInCart = CoffeShopContext.Ins.Carts
									   .FirstOrDefault(c => c.UserId == userId && c.MenuId == idMenu);

					if (menuInCart != null)
					{
						CoffeShopContext.Ins.Carts.Remove(menuInCart);
					}
					else
					{
						Console.WriteLine($"Menu item with id {idMenu} not found in the cart for user {userId}.");
					}
				}

				CoffeShopContext.Ins.SaveChanges();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}




	}
}
