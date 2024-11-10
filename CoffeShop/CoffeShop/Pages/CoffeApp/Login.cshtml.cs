using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp
{
    public class LoginModel : PageModel
    {
        private readonly UserService userService;

        public LoginModel(UserService userService)
        {
            this.userService = userService;
        }

        [BindProperty]
		public string Username { get; set; }

		[BindProperty]
		public string Password { get; set; }

		public string? ErrorMessage { get; set; }
		public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Username and Password are required.";
                return Page();
            }

            var user = userService.GetUserByUsername(Username);
            if (user == null)
            {
                ErrorMessage = "User not found.";
                return Page();
            }

            if(user.Status == "Blocked")
            {
				ErrorMessage = "Your account is blocked. Please contact support.";
				return Page();
			}

            if (BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {

				HttpContext.Session.SetInt32("UserId", user.UserId); 
				HttpContext.Session.SetString("UserRole", user.Role); 
				HttpContext.Session.SetString("Username", user.FullName);
				HttpContext.Session.SetInt32("Point", (int)user.Poins);

				return RedirectToPage("/CoffeApp/Home"); 
            }
            else
            {
                ErrorMessage = "Incorrect password.";
                return Page();
            }
        }
    }
}
