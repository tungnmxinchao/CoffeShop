using CoffeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
			HttpContext.Session.Remove("UserId");
			HttpContext.Session.Remove("UserRole");
			HttpContext.Session.Remove("Username");
			HttpContext.Session.Remove("Point");

			return RedirectToPage("/CoffeApp/Home");
		}
    }
}
