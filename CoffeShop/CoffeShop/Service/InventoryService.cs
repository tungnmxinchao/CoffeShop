using CoffeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeShop.Service
{
	public class InventoryService
	{

		public Inventory? GetInventoryItem(int? itemId)
		{
			return CoffeShopContext.Ins.Inventories.FirstOrDefault(x => x.ItemId == itemId);
		}

		public bool UpdateInventory(Inventory inventoryItem)
		{
			try
			{
				CoffeShopContext.Ins.Inventories.Update(inventoryItem);
				CoffeShopContext.Ins.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
