using CoffeShop.Models;

namespace CoffeShop.Service
{
	public class OrderDetailsService
	{
		public List<OrderDetail> FindByOrderId(int orderId)
		{
			return CoffeShopContext.Ins.OrderDetails.Where(x => x.OrderId == orderId).ToList();
		}
	}
}
