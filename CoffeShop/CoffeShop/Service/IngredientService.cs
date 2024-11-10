using CoffeShop.Models;

namespace CoffeShop.Service
{
	public class IngredientService
	{

		public List<ProductIngredient> FindIngredientsByMenuId(int? menuId)
		{
			return CoffeShopContext.Ins.ProductIngredients
				.Where(x => x.MenuId == menuId).ToList();
		}
	}
}
