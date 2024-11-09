using System.Net;
using System.Net.Mail;
using CoffeShop.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks; 

namespace CoffeShop.Service
{
	public class EmailService
	{
		private readonly IConfiguration _config;

		public EmailService(IConfiguration config)
		{
			_config = config;
		}

		public async Task SendOrderConfirmationEmail(string toEmail, string userName, Order order)
		{
			var smtpServer = _config["EmailSettings:SmtpServer"];
			var port = int.Parse(_config["EmailSettings:Port"]);
			var senderEmail = _config["EmailSettings:SenderEmail"];
			var senderName = _config["EmailSettings:SenderName"];
			var password = _config["EmailSettings:Password"];

			var message = new MailMessage();
			message.From = new MailAddress(senderEmail, senderName);
			message.To.Add(new MailAddress(toEmail));
			message.Subject = "Order Confirmation";
			message.IsBodyHtml = true;
			message.Body = GenerateEmailBody(userName, order);


			using (var client = new SmtpClient(smtpServer, port))
			{
				client.Credentials = new NetworkCredential(senderEmail, password);
				client.EnableSsl = true;
				await client.SendMailAsync(message);  
			}
		}

		public async Task SendLowInventoryNotification(List<string> lowInventoryItems, string staffEmail)
		{
			var smtpServer = _config["EmailSettings:SmtpServer"];
			var port = int.Parse(_config["EmailSettings:Port"]);
			var senderEmail = _config["EmailSettings:SenderEmail"];
			var senderName = _config["EmailSettings:SenderName"];
			var password = _config["EmailSettings:Password"];

			var message = new MailMessage();
			message.From = new MailAddress(senderEmail, senderName);
			message.To.Add(new MailAddress(staffEmail));
			message.Subject = "Low Inventory Notification";
			message.IsBodyHtml = true;
			message.Body = GenerateLowInventoryEmailBody(lowInventoryItems);

			using (var client = new SmtpClient(smtpServer, port))
			{
				client.Credentials = new NetworkCredential(senderEmail, password);
				client.EnableSsl = true;
				await client.SendMailAsync(message);
			}
		}

		private string GenerateEmailBody(string userName, Order order)
		{
			var items = string.Join("<br>", order.OrderDetails.Select(
				d => $"{d.Quantity} x {d.Menu.Name} - ${d.Price}"
			));

			return $@"
                <h2>Thank you for your order, {userName}!</h2>
                <p>Order ID: {order.OrderId}</p>
                <p>Total Price: ${order.TotalPrice}</p>
                <h4>Order Details:</h4>
                <p>{items}</p>
                <p>We hope you enjoy your meal!</p>
            ";
		}

		private string GenerateLowInventoryEmailBody(List<string> lowInventoryItems)
		{
			var itemsList = string.Join("<br>", lowInventoryItems);

			return $@"
                <h2>Urgent: Low Inventory Notification</h2>
                <p>The following items have low inventory levels:</p>
                <ul>
                    {string.Join("", lowInventoryItems.Select(item => $"<li>{item}</li>"))}
                </ul>
                <p>Please restock these items as soon as possible.</p>
            ";
		}
	}
}
