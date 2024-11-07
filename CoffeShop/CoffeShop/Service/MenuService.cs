using CoffeShop.Models;

namespace CoffeShop.Service
{
	public class MenuService
	{
		public List<Menu> FindAll()
		{
			return CoffeShopContext.Ins.Menus.ToList();
		}

		public Menu GetById(int id)
		{
			var menu = CoffeShopContext.Ins.Menus.Find(id);

			if(menu == null)
			{
				throw new Exception("ID product not found");
			}

			return menu;
		}

	}
}
