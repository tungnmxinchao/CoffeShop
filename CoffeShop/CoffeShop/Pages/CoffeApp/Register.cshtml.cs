using System.ComponentModel.DataAnnotations;
using CoffeShop.Models;
using CoffeShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeShop.Pages.CoffeApp
{
    public class RegisterModel : PageModel
    {
		private readonly UserService userService;
		private readonly EmailService emailService;
		public RegisterModel(UserService userService, EmailService emailService)
		{
			this.userService = userService;
			this.emailService = emailService;
		}

		[BindProperty]
		[Required(ErrorMessage = "Username is required")]
		public string Username { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Password is required")]
		public string Password { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Full Name is required")]
		public string FullName { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email format")]
		public string Email { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Phone number is required")]
		[Phone(ErrorMessage = "Invalid phone number format")]
		public string Phone { get; set; }

		public string StatusMessage { get; set; }
		public IActionResult OnPost()
        {
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var existingUser = userService.CheckExistUser(Username, Email);
			if (existingUser != null)
			{
				StatusMessage = "Username or email already exists";
				return Page();
			}

			string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);

			var newUser = new User
			{
				Username = Username,
				Password = hashedPassword, 
				FullName = FullName,
				Email = Email,
				Phone = Phone,
				CreatedAt = DateTime.Now,
				Status = "Blocked", 
				Role = "Customer" 
			};

			bool isAdded = userService.AddUser(newUser);
			if (isAdded)
			{
				StatusMessage = "Please check your email to active account!";

				Task.Run(() => emailService.SendAccountConfirmationEmail(Email, Username));
				return Page();
			}
			else
			{
				StatusMessage = "Registration failed. Please try again.";
				return Page();
			}
		}
    }
}
