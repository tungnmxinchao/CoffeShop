using CoffeShop.Models;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp
{
    public class HomeModel : PageModel
    {
        private readonly MenuService menuService;

		public HomeModel(MenuService menuService)
		{
			this.menuService = menuService;
		}

		public List<Menu> Menus { get; set; }
        public IActionResult OnGet()
        {
			Menus = menuService.FindAll();

			return Page();

		}
    }
}
