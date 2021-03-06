﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using WebApplication1.Controllers;

namespace WebApplication1.Models
{
	public class Invoice
	{
		public Invoice()
		{
			Lines = new List<InvoiceLine>();
			Payments = new List<Payment>();
		}
		public string CustomerId { get; set; }
		public string CustomerName { get; set; }
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

		public int RecordId { get; set; }

		public string SalesPersonId { get; set; }

		public double TaxAmount1 { get; set; }

		public DateTime InvoiceDate { get; set; }

		public ChargeDetails ChargeDetail { get; set; }

		[JsonIgnore]
		public double SubTotal
		{
			get { return Total + TotalDiscount; }
		}

		public double TotalDiscount { get; set; }

		public double TaxAmount2 { get; set; }

		public List<InvoiceLine> GetCompinedLines()
		{
			var lines = Lines.ToList();
			if(Payments.Any(x=> x.Amount != 0))
				lines.Add(new InvoiceLine
				{
					TransCode = "N",
					Qty = 0,
					Description = "--------------------"
				});

			lines.AddRange(Payments.Where(x=> x.Amount != 0).Select(x=> new InvoiceLine
			{
				Description = x.PaymentType.Id,
				TransCode = "Y",
				Price = x.Amount,
				Qty = 0,
				TaxCode = "3",
			}));

			int i = 1;
			lines.ForEach(x =>
			{
				x.LineOrder = i++;
				x.ParentRecordId = RecordId;
			});
			return lines;
		}

		public void ParseLines(IEnumerable<InvoiceLine> lines)
		{
			bool foundSeperator = false;
			foreach (var invoiceLine in lines)
			{
				bool isSeperator = invoiceLine.Description == "--------------------" && invoiceLine.TransCode == "N";
				if (isSeperator)
				{
					foundSeperator = true;
					continue;
				}
				if(foundSeperator)
					ParseBottomLine(invoiceLine);
				else 
					Lines.Add(invoiceLine);
			}

			var paymentsTotal = Payments.Sum(x => x.Amount);
			var change = paymentsTotal - Total;

			if (change != 0)
			{
				Payments.Where(x => x.PaymentType.Id == "Cash").FirstOrDefault().Change = change;
			}

		}

		void ParseBottomLine(InvoiceLine line)
		{
			if (line.TransCode.ToLower() == "y")
				Payments.Add(new Payment
				{
					Amount = line.Price,
					PaymentType = PaymentTypeController.PaymentTypes.Where(x=> x.Id == line.Description).FirstOrDefault(),
					
				});
			else 
				Console.WriteLine(line);
		}



	}
}