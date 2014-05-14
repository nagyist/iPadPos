using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class Payment
	{
		public int Id { get; set; }
		public PaymentType PaymentType { get; set; }
		public double Amount { get; set; }
		public double Change { get; set; }
	}
}