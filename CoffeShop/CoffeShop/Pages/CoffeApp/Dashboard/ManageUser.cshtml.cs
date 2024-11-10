using CoffeShop.Models;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
    public class ManageUserModel : PageModel
    {
        private readonly UserService userService;

		public ManageUserModel(UserService userService)
		{
			this.userService = userService;
		}

		public List<User> Users { get; set; }

		public IActionResult OnGet()
        {
			Users = userService.FindAll();

			return Page();

		}
    }
}
