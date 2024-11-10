namespace CoffeShop.DTOs
{
	public class ProfitLossReportPerDay
	{
		public DateTime? OrderDate { get; set; }
		public decimal TotalRevenue { get; set; }
		public decimal TotalMaterialCost { get; set; }
		public decimal ProfitLoss { get; set; }

	}
}
