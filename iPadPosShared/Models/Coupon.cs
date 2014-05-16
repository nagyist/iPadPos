using System;

namespace iPadPos
{
	public class Coupon : Item
	{
		public Coupon ()
		{
			ItemType = ItemType.Coupon;
		}

		public bool SelectedItemsOnly {get;set;}

		public float DiscountPercent {get;set;}

		public DateTime? StartDate {get;set;}

		public DateTime? EndDate {get;set;}

		public bool IsValidToady
		{
			get{
				if (StartDate == null)
					return true;
				if (EndDate == null)
					return true;
				var now = DateTime.Now;
				return now >= StartDate.Value && now <= EndDate.Value;
			}
		}
	}
}

