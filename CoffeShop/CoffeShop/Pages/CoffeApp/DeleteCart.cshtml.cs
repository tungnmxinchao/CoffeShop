using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp
{
	public class DeleteCartModel : PageModel
	{
		private readonly CartService cartService;

		public DeleteCartModel(CartService cartService)
		{
			this.cartService = cartService;
		}

		[BindProperty(SupportsGet = true)]
		public int MenuId { get; set; }

		public IActionResult OnGet()
		{
			var userId = HttpContext.Session.GetInt32("UserId");

			int[] ArrayMenuId = new int[] { MenuId };
			if (cartService.RemoveCart(userId, ArrayMenuId))
			{
				return RedirectToPage("/CoffeApp/ViewCart");
			}

			return Page();

		}
	}
}
