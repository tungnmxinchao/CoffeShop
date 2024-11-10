using CoffeShop.Models;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp.Dashboard
{
	public class UpdateUserModel : PageModel
	{
		private readonly UserService _userService;

		public UpdateUserModel(UserService userService)
		{
			_userService = userService;
		}

		[BindProperty(SupportsGet = true)]
		public int UserId { get; set; }

		[BindProperty]
		public User User { get; set; }

		public IActionResult OnGet()
		{
			User = _userService.FindUserById(UserId);
			if (User == null)
			{
				return NotFound();
			}
			return Page();
		}

		public IActionResult OnPost()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var userToUpdate = _userService.FindUserById(UserId);
			if (userToUpdate == null)
			{
				return NotFound();
			}

			userToUpdate.Status = User.Status;
			userToUpdate.Role = User.Role;
			_userService.UpdateUser(userToUpdate);

			return RedirectToPage("/CoffeApp/Dashboard/ManageUser");
		}
	}
}
