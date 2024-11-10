using CoffeShop.Models;

namespace CoffeShop.Service
{
    public class StaffService
    {
        public List<User> FindAllStaff()
        {
            return CoffeShopContext.Ins.Users.Where(x => x.Role == "Staff").ToList();
        }
    }
}
