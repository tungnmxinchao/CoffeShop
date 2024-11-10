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

		public async Task SendAccountConfirmationEmail(string toEmail, string userName)
		{
			var smtpServer = _config["EmailSettings:SmtpServer"];
			var port = int.Parse(_config["EmailSettings:Port"]);
			var senderEmail = _config["EmailSettings:SenderEmail"];
			var senderName = _config["EmailSettings:SenderName"];
			var password = _config["EmailSettings:Password"];

			var message = new MailMessage
			{
				From = new MailAddress(senderEmail, senderName),
				Subject = "Account Confirmation",
				IsBodyHtml = true,
				Body = GenerateAccountConfirmationEmailBody(userName)
			};

			message.To.Add(new MailAddress(toEmail));

			using (var client = new SmtpClient(smtpServer, port))
			{
				client.Credentials = new NetworkCredential(senderEmail, password);
				client.EnableSsl = true;
				await client.SendMailAsync(message);
			}
		}

		public async Task SendCancelOrder(string reason, string userEmail, string userFullName, Order order)
		{
			var smtpServer = _config["EmailSettings:SmtpServer"];
			var port = int.Parse(_config["EmailSettings:Port"]);
			var senderEmail = _config["EmailSettings:SenderEmail"];
			var senderName = _config["EmailSettings:SenderName"];
			var password = _config["EmailSettings:Password"];

			var message = new MailMessage();
			message.From = new MailAddress(senderEmail, senderName);
			message.To.Add(new MailAddress(userEmail));  
			message.Subject = "Order Cancellation Notice";
			message.IsBodyHtml = true;

		
			message.Body = GenerateCancellationEmailBody(reason, userFullName, order);

			using (var client = new SmtpClient(smtpServer, port))
			{
				client.Credentials = new NetworkCredential(senderEmail, password);
				client.EnableSsl = true;
				await client.SendMailAsync(message);
			}
		}

		private string GenerateCancellationEmailBody(string reason, string userFullName, Order order)
		{
			return $@"
			<h2>Dear {userFullName},</h2>
			<p>Your order (Order ID: {order.OrderId}) has been cancelled.</p>
			<p><strong>Reason for Cancellation:</strong> {reason}</p>
			<p>We apologize for the inconvenience. We will contact you as soon as possibal to back money</p>
			<p>Thank you for understanding.</p>
			";
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

		private string GenerateAccountConfirmationEmailBody(string userName)
		{
			long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

			string confirmationLink = $"https://localhost:7067/CoffeApp/ActiveAccount?username={userName}&timestamp={timestamp}";

			return $@"
            <h2>Hello {userName},</h2>
            <p>Thank you for registering with us!</p>
            <p>Please click the link below to activate your account:</p>
            <p><a href='{confirmationLink}'>Activate Account</a></p>
            <p>If you did not register, please ignore this email.</p>
        ";
		}


	}
}
