using CoffeShop.Models;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp
{
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
			cartService.AddToCart(MenuId, 1);

			return RedirectToPage("/CoffeApp/ViewCart");

		}
	}
}
