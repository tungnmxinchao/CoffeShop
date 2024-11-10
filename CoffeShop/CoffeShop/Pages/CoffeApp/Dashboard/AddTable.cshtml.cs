using CoffeShop.Models;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    public class AddTableModel : PageModel
    {

        private readonly TableService tableService;
        private readonly StaffService staffService;
        

        public AddTableModel(TableService tableService, StaffService staffService)
        {
            this.tableService = tableService;
            this.staffService = staffService;
        }

        [BindProperty]
        public Table Table { get; set; }

        public List<User> Staffs { get; set; }

        public string Message {  get; set; }
        public IActionResult OnGet()
        {

            Staffs = staffService.FindAllStaff();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (tableService.AddTable(Table))
            {
                Message = "Add Successfully!";
            }
            else
            {
                Message = "Add Failed!";
            }
            Staffs = staffService.FindAllStaff();
            return Page();
        }

    }
}
