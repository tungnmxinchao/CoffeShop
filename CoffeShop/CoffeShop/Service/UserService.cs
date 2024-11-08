using CoffeShop.Models;

namespace CoffeShop.Service
{
	public class UserService
	{

		public User FindUserById(int id)
		{
			var user = CoffeShopContext.Ins.Users.Find(id);

			if (user == null)
			{
				throw new Exception("ID Not Found!");
			}

			return user;
		}
	}
}
