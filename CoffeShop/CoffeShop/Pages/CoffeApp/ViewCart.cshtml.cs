
using CoffeShop.Models;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp
{
    public class ViewCartModel : PageModel
    {

        private readonly CartService cartService;

		public ViewCartModel(CartService cartServicem )
		{
			this.cartService = cartServicem;
		}

		public List<Cart> Carts { get; set; }

		public decimal TotalCart { get; set; }

		public IActionResult OnGet()
        {

			Carts = cartService.FindAllCartByUserId(1);

			TotalCart = Carts.Sum(cart => cart.PriceAtAdd);

			return Page();
        }
    }
}
