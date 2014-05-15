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
		public float DiscountPercent {get;set;}
	}
}