using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using MailKit.Net.Smtp;
using MimeKit;
using Simpler;
using WebApplication1.Models;

namespace WebApplication1.Tasks
{
	public class SendEmailTask : SimpleTask
	{
		public Invoice Invoice { get; set; }
		public override void Execute()
		{
			if (!IsValidEmail(Invoice.Customer.Email))
				return;
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("Anchorage Kid to Kid", "anchorage@kidtokid.com"));
			message.To.Add(new MailboxAddress(Invoice.Customer.ToString(), Invoice.Customer.Email));
			message.Subject = "Receipt: " + Invoice.Id;

			var body = new BodyBuilder();
			body.HtmlBody = GetHtmlBody(Invoice);
			body.TextBody = "";
			message.Body = body.ToMessageBody();

			using (var client = new SmtpClient())
			{
				var userName = SharedDb.PosimDb.GetString("SELECT StringValue FROM  DBA.BYR_PREFS where PrefTitle = 'emailUN'");
				var pw = SharedDb.PosimDb.GetString("SELECT StringValue FROM  DBA.BYR_PREFS where PrefTitle = 'emailPW'");
				var credentials = new NetworkCredential(userName, pw);

				// Note: if the server requires SSL-on-connect, use the "smtps" protocol instead
				var uri = new Uri("smtp://smtp.gmail.com:587");

				using (var cancel = new CancellationTokenSource())
				{
					client.Connect(uri, cancel.Token);

					// Note: since we don't have an OAuth2 token, disable
					// the XOAUTH2 authentication mechanism.
					client.AuthenticationMechanisms.Remove("XOAUTH2");

					// Note: only needed if the SMTP server requires authentication
					client.Authenticate(credentials, cancel.Token);

					client.Send(message, cancel.Token);
					client.Disconnect(true, cancel.Token);
				}
			}
		}
		 public static bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return true;
			}
			catch
			{
				return false;
			}
		}
		private static string GetHtmlBody(Invoice invoice)
		{
			string filePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath,"Content", "invoice.htm");// Server.MapPath(Url.Content("~/Content/Images/Image.jpg"));
			var customer = invoice.Customer;
			//CustomerDatabase.RefreshCustomer(ref customer);
			var content = File.ReadAllText(filePath);
			content = content.Replace("[SalesPerson]", invoice.SalesPersonId);
			content = content.Replace("[InvoiceID]", invoice.Id);
			content = content.Replace("[Date]", DateTime.Now.ToString());
			content = content.Replace("[CustomerName]", invoice.Customer.ToString());
			content = content.Replace("[Email]", invoice.Customer.Email);
			content = content.Replace("[OnAccountAmt]", invoice.Customer.OnAccount.ToString("C2"));
			content = content.Replace("[DATA]", CreateInvoiceData(invoice));
			//content = content.Replace("[Points]", customer.LoyaltyPoints.ToString());
			return content;
		}

		private static string GetPlainBody(Invoice invoice)
		{
			return "";// Receipt40Col.GetStandardReceipt(invoice);
		}
		private static string CreateInvoiceData(Invoice invoice)
		{
			string lineString = "<tr class=\"{0}\"><td>{1}</td><td>{2}</td><td>{3}</td></tr>";
			StringBuilder sb = new StringBuilder();
			foreach (var line in invoice.Lines)
			{
				bool even = invoice.Lines.IndexOf(line) % 2 == 0;
				sb.AppendLine(string.Format(lineString, even ? "even" : "odd", HttpUtility.HtmlEncode(line.Description), line.Qty, line.Price.ToString("C2")));
				if (line.Discount != 0)
					sb.AppendLine(string.Format(lineString, even ? "even" : "odd", "", "*Clearance Item", "", line.Discount.ToString("C2")));
			}
			sb.AppendLine(string.Format(lineString, "even", "", "", "Sub Total:", invoice.SubTotal.ToString("C2")));
			sb.AppendLine(string.Format(lineString, "even", "", "", "Discount:", invoice.TotalDiscount.ToString("C2")));
			//sb.AppendLine(string.Format(lineString, "even", "", "", "Tax:", invoice.TotalTax.ToString("C2")));
			sb.AppendLine(string.Format(lineString, "odd", "", "", bold("Total:"), bold(invoice.Total.ToString("C2"))));

			sb.AppendLine("<tr/>");

			sb.AppendLine(string.Format(lineString, "even", "Payments Applied:", "", "", ""));

			foreach (var payment in invoice.Payments)
			{
				bool even = invoice.Payments.IndexOf(payment) % 2 == 0;
				sb.AppendLine(string.Format(lineString, even ? "even" : "odd", "", "", payment.PaymentType.Description, payment.Amount.ToString("C2")));
				var change = Math.Abs(payment.Change);
				if (change != 0)
				{
					sb.AppendLine(string.Format(lineString, even ? "even" : "odd", "", "", "Change:", change.ToString("C2")));
				}
			}

			return sb.ToString();

		}
		static string bold(string str)
		{
			return string.Format("<strong>{0}</strong>", str);
		}
		
	}
}