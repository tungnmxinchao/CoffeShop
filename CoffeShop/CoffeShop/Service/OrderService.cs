using CoffeShop.Models;

namespace CoffeShop.Service
{
	public class OrderService
	{

		public bool CreateOrder(Order order)
		{

			if (order == null) return false;

			try
			{
				CoffeShopContext.Ins.Orders.Add(order);
				CoffeShopContext.Ins.SaveChanges();

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}
	}
}
