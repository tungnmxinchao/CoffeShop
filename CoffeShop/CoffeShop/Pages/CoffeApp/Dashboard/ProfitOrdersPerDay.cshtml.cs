using CoffeShop.DTOs;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    public class ProfitOrdersPerDayModel : PageModel
    {
        private readonly ReportService reportService;

		public ProfitOrdersPerDayModel(ReportService reportService)
		{
			this.reportService = reportService;
		}

        public List<ProfitLossReportPerDay> ProfitOrdersPerDay { get; set; }

		[BindProperty(SupportsGet = true)]
		public DateTime? StartDate { get; set; }

		[BindProperty(SupportsGet = true)]
		public DateTime? EndDate { get; set; }

		public IActionResult OnGet()
		{
			if (StartDate.HasValue && EndDate.HasValue)
			{
				ProfitOrdersPerDay = reportService.GenerateProfitLossReportPerDay()
					.Where(r => r.OrderDate >= StartDate.Value && r.OrderDate <= EndDate.Value)
					.ToList();
			}
			else
			{
				ProfitOrdersPerDay = reportService.GenerateProfitLossReportPerDay();
			}

			return Page();
		}
	}
}
