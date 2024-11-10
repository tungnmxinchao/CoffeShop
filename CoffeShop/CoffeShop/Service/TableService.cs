using CoffeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeShop.Service
{
	public class TableService
	{

		public List<Table> FindAllTable()
		{
			return CoffeShopContext.Ins.Tables.Include(x => x.WaiterNavigation).ToList();
		}
		public bool UpdateTable(Table table)
		{
			if (table == null)
			{
				throw new Exception("Table not found!");
			}

			try
			{
				CoffeShopContext.Ins.Tables.Update(table);
				CoffeShopContext.Ins.SaveChanges();

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public Table GetTable(int id)
		{
			var table = CoffeShopContext.Ins.Tables.Find(id);

			if (table == null)
			{
				throw new Exception("Table not found!");
			}
			return table;
		}

        public bool AddTable(Table table)
        {
            if (table == null)
            {
				return false;
            }

            try
            {
                CoffeShopContext.Ins.Tables.Add(table);
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
