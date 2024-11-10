using CoffeShop.DTOs;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    public class ProfitOrdersModel : PageModel
    {
        private readonly ReportService reportService;

		public ProfitOrdersModel(ReportService reportService)
		{
			this.reportService = reportService;
		}

        public List<ProfitLossReport> ProfitOrders { get; set; }

		public IActionResult OnGet()
        {
			ProfitOrders = reportService.GenerateProfitLossReport();

			return Page();
        }
    }
}
