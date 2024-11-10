using CoffeShop.DTOs;
using CoffeShop.Filter;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    [RequireManager]
    public class ProfitOrdersPerMonthModel : PageModel
    {
		private readonly ReportService reportService;

		public ProfitOrdersPerMonthModel(ReportService reportService)
		{
			this.reportService = reportService;
		}
		public List<ProfitLossReportPerMonth> ProfitOrdersPerMonth { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }


		public IActionResult OnGet(DateTime? startDate, DateTime? endDate)
		{
			if (startDate.HasValue)
			{
				StartDate = startDate;
			}
			if (endDate.HasValue)
			{
				EndDate = endDate;
			}

			var allReports = reportService.GenerateProfitLossReportPerMonth();

			int? startYear = StartDate?.Year;
			int? startMonth = StartDate?.Month;
			int? endYear = EndDate?.Year;
			int? endMonth = EndDate?.Month;

			ProfitOrdersPerMonth = allReports
				.Where(report =>
					(!startYear.HasValue ||
					 (report.OrderYear > startYear || (report.OrderYear == startYear && report.OrderMonth >= startMonth))) &&
					(!endYear.HasValue ||
					 (report.OrderYear < endYear || (report.OrderYear == endYear && report.OrderMonth <= endMonth))))
				.ToList();

			return Page();
		}
	}
}
