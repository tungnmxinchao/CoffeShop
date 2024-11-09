using CoffeShop.Models;

namespace CoffeShop.Service
{
	public class UserService
	{

		public User FindUserById(int? id)
		{
			var user = CoffeShopContext.Ins.Users.Find(id);

			if (user == null)
			{
				throw new Exception("ID Not Found!");
			}

			return user;
		}

		public void AddLoyaltyPoints(int userId, int pointsToAdd)
		{
			var user = CoffeShopContext.Ins.Users.Find(userId);

			if (user == null)
			{
				throw new Exception("User Not Found!");
			}

			if (user.Poins != null)
			{
				user.Poins += pointsToAdd;
			}
			else
			{
				throw new Exception("User does not have a loyalty points record.");
			}

			CoffeShopContext.Ins.SaveChanges();
		}

		public void UpdateUser(User updatedUser)
		{
			var user = CoffeShopContext.Ins.Users.Find(updatedUser.UserId);

			if (user == null)
			{
				throw new Exception("User Not Found!");
			}

			user.Username = updatedUser.Username;
			user.Password = updatedUser.Password;
			user.FullName = updatedUser.FullName;
			user.Email = updatedUser.Email;
			user.Phone = updatedUser.Phone;
			user.Role = updatedUser.Role;
			user.Poins = updatedUser.Poins;

			CoffeShopContext.Ins.SaveChanges();
		}

		public User CheckExistUser(string username, string email)
		{
			var user = CoffeShopContext.Ins.Users
				.FirstOrDefault(u => u.Username == username || u.Email == email);

			return user;
		}

		public bool AddUser(User newUser)
		{
			try
			{
				CoffeShopContext.Ins.Users.Add(newUser);
				CoffeShopContext.Ins.SaveChanges();
				return true; 
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false; 
			}
		}

		public User GetUserByUsername(string username)
		{
			var user = CoffeShopContext.Ins.Users
				.FirstOrDefault(u => u.Username == username);

			if (user == null)
			{
				throw new Exception("User not found.");
			}

			return user;
		}


	}
}
