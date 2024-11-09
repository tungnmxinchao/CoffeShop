using CoffeShop.Filter;
using CoffeShop.Models;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp
{
	[RequireUser]
	public class AddToCartModel : PageModel
	{

		private readonly CartService cartService;

		public AddToCartModel(CartService cartService)
		{
			this.cartService = cartService;
		}

		[BindProperty(SupportsGet = true)]
		public int MenuId { get; set; }

		public IActionResult OnGet()
		{
			var userId = HttpContext.Session.GetInt32("UserId");

			cartService.AddToCart(MenuId, 1 , userId);

			return RedirectToPage("/CoffeApp/ViewCart");

		}
	}
}
