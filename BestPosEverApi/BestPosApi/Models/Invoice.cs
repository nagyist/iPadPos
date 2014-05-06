using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class Invoice
	{
		public Invoice()
		{
			Lines = new List<InvoiceLine>();
			Payments = new List<Payment>();
		}
		public Customer Customer { get; set; }

		public List<Payment> Payments { get; set; } 

		public List<InvoiceLine> Lines { get; set; }

		public double Discount { get; set; }

		public int DiscountType { get; set; }

		public string Id { get; set; }

		public double Taxable { get; set; }

		public double Total { get; set; }

		public int PostStatus { get; set; }

		public string RegisterId { get; set; }

		public int RecorId { get; set; }

		public string SalesPersonId { get; set; }

		public double TaxAmount1 { get; set; }

		public double TaxAmount2 { get; set; }

		public List<InvoiceLine> GetCompinedLines()
		{
			var lines = Lines.ToList();
			lines.Add(new InvoiceLine
			{
				TransCode = "N",
				Qty = 0,
				Description = "--------------------"
			});

			lines.AddRange(Payments.Select(x=> new InvoiceLine
			{
				Description = x.PaymentType,
				TransCode = "Y",
				Price = x.Amount,
				Qty = 0,
			}));

			int i = 1;
			lines.ForEach(x => x.LineOrder = i++);
			return lines;
		}

	}
}