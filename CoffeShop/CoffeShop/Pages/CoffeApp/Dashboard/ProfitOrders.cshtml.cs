using CoffeShop.DTOs;
using CoffeShop.Filter;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    [RequireManager]
    public class ProfitOrdersModel : PageModel
    {
        private readonly ReportService reportService;

		public ProfitOrdersModel(ReportService reportService)
		{
			this.reportService = reportService;
		}

		[BindProperty(SupportsGet = true)]
		public DateTime? StartDate { get; set; }

		[BindProperty(SupportsGet = true)]
		public DateTime? EndDate { get; set; }

		public List<ProfitLossReport> ProfitOrders { get; set; }

		public IActionResult OnGet()
		{
			
			ProfitOrders = reportService.GenerateProfitLossReport();

			if (StartDate.HasValue && EndDate.HasValue)
			{
				ProfitOrders = ProfitOrders
					.Where(order => order.OrderDate >= StartDate.Value && order.OrderDate <= EndDate.Value)
					.ToList();
			}

			return Page();
		}
	}
}
