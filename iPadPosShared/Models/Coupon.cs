using System;

namespace iPadPos
{
	public class Coupon : Item
	{
		public Coupon ()
		{
			ItemType = ItemType.Coupon;
		}
		public float DiscountPercent {get;set;}
	}
}

