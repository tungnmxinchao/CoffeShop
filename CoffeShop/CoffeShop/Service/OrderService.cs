using CoffeShop.Models;
using Microsoft.EntityFrameworkCore;

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

		public List<Order> FindAllOrdersByUserId(int? userId)
		{
			return CoffeShopContext.Ins.Orders.Where(x => x.UserId == userId)
				.OrderByDescending(x => x.OrderId).ToList();
		}

		public Order? FindOrderById(int id)
		{
			var order = CoffeShopContext.Ins.Orders.Find(id);

			if(order != null)
			{
				return order;
			}

			return null;
		}

		public List<Order> FindAllOrders()
		{
			return CoffeShopContext.Ins.Orders.Include(x => x.User).ToList();
		}

		public void UpdateOrder(Order order)
		{
			
			if(order == null) return;

			CoffeShopContext.Ins.Orders.Update(order);
			CoffeShopContext.Ins.SaveChanges();

		}
	}
}
