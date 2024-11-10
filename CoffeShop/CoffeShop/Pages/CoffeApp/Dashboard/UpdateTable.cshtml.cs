using CoffeShop.Models;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    public class UpdateTableModel : PageModel
    {
        private readonly TableService tableService;
        private readonly StaffService staffService;

        public UpdateTableModel(TableService tableService, StaffService staffService)
        {
            this.tableService = tableService;
            this.staffService = staffService;
        }
        public List<User> Staffs { get; set; }

        [BindProperty]
        public Table Table { get; set; }

        public IActionResult OnGet(int id)
        {
            Table = tableService.GetTable(id);
            Staffs = staffService.FindAllStaff();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (tableService.UpdateTable(Table))
            {
                Table = tableService.GetTable(Table.TableId);
                Staffs = staffService.FindAllStaff();
            }
            return Page();

        }


    }
}
