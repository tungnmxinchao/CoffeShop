namespace CoffeShop.DTOs
{
	public class ProfitLossReportPerMonth
	{
		public int? OrderYear { get; set; } 
		public int? OrderMonth { get; set; } 
		public decimal TotalRevenue { get; set; } 
		public decimal TotalMaterialCost { get; set; }
		public decimal ProfitLoss { get; set; } 
	}
}
