using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp
{
    public class UpdateCartModel : PageModel
    {
        private readonly CartService cartService;

		public UpdateCartModel(CartService cartService)
		{
			this.cartService = cartService;
		}

        [BindProperty(SupportsGet = true)]
        public string Quantities { get; set; }

		public IActionResult OnGet()
        {

			string[] ArrayQuantiies = Quantities.Split(',');

			var Carts = cartService.FindAllCartByUserId(1);

			if(cartService.UpdateCart(Carts, ArrayQuantiies))
			{
				return RedirectToPage("/CoffeApp/ViewCart");
			}
			return Page();


		}
    }
}
