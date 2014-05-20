using System;
using Newtonsoft.Json;
using SQLite;

namespace iPadPos
{

	public class Item : BaseModel
	{


		#region private properties

		private string brand;
		private double buyAmt;
		private int cogs;
		private string datePrice;
		private string description;
		private string gender;
		private string grade;
		private string itemID;
		private string longItemID;
		private string misc1;
		private string misc2;
		private string misc3;
		private string misc4;
		private string misc5;
		private double orgBuyAmt;
		private int printCode;
		private int qty;
		private string season;
		private double sellAmt;
		private string taxCode;

		#endregion

		#region public properties

		[PrimaryKey]
		public string ItemID {
			get { return itemID; }
			set { ProcPropertyChanged (ref itemID, value); }
		}

		public string Misc1 {
			get { return misc1; }
			set { ProcPropertyChanged (ref misc1, value); }
		}

		public string Misc2 {
			get { return misc2; }
			set { ProcPropertyChanged (ref misc2, value); }
		}

		public string Misc3 {
			get { return misc3; }
			set { ProcPropertyChanged (ref misc3, value); }
		}

		public string Misc4 {
			get { return misc4; }
			set { ProcPropertyChanged (ref misc4, value); }
		}

		public string Misc5 {
			get { return misc5; }
			set { ProcPropertyChanged (ref misc5, value); }
		}

		public string Season {
			get { return season; }
			set { ProcPropertyChanged (ref season, value); }
		}

		public string TaxCode {
			get { return taxCode; }
			set { ProcPropertyChanged (ref taxCode, value); }
		}

		public string Description {
			get { return description; }
			set { ProcPropertyChanged (ref description, value); }
		}

		public int Qty {
			get { return qty; }
			set { ProcPropertyChanged (ref qty, value); }
		}


		public double Price { get; set; }

		public double Cost { get; set; }


		public string TransCode { get; set; }
	
		public int OnHand {get;set;}

		[JsonIgnore]
		[Indexed]
		public ItemType ItemType {get;set;}

		public bool UseAlterate ()
		{
			var cpn = this as Coupon;
			if (cpn == null)
				return false;
			var useAlt = cpn.DiscountPercent == 0 && cpn.Price == 0;
			return useAlt;
		}

		#endregion
	}

	public enum ItemType
	{
		Item,
		NewCustomerTracking,
		NewProduct,
		Coupon,
	}
}

