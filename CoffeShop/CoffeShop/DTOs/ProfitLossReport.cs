namespace CoffeShop.DTOs
{
	public class ProfitLossReport
	{
		public int OrderId { get; set; }
		public decimal TotalRevenue { get; set; }
		public decimal TotalMaterialCost { get; set; }
		public decimal ProfitLoss { get; set; }
	}
}
