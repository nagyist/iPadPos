using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Models
{
	public class Coupon : Item
	{
		public Coupon ()
		{

		}
		public bool SelectedItemsOnly { get; set; }

		public float DiscountPercent { get; set; }

		public bool ManualDiscount { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }
	}
}