using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class ChargeDetails
	{
		public string InvoiceId { get; set; }
		public ChargeDetails()
		{
			Signature = new Signature();
		}
		public double Amount { get; set; }

		public string Token { get; set; }

		public string ReferenceID { get; set; }

		public bool IsRefunded { get; set; }

		public double AmountRefunded { get; set; }

		public DateTime Created { get; set; }

		[Newtonsoft.Json.JsonIgnore]
		public int LocalInvoiceId { get; set; }

		public Signature Signature { get; set; }

	}
}