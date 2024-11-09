using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp
{
    public class ActiveAccountModel : PageModel
    {
		private readonly UserService userService;

		public ActiveAccountModel(UserService userService)
		{
			this.userService = userService;
		}

		public string StatusMessage { get; set; }

		public IActionResult OnGet(string username, long timestamp)
        {
			var expirationWindow = TimeSpan.FromHours(24).TotalSeconds;

			long currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

			if (Math.Abs(currentTimestamp - timestamp) > expirationWindow)
			{
				StatusMessage = "The confirmation link has expired.";
				return Page(); 
			}

	
			var user = userService.GetUserByUsername(username);
			if (user != null)
			{
	
				user.Status = "Active";
				userService.UpdateUser(user);

				StatusMessage = "Your account has been successfully activated.";
			}
			else
			{
				StatusMessage = "User not found.";
			}

			return Page();
		}
    }
}
