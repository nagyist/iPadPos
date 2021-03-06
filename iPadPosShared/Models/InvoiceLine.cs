﻿using System;
using Newtonsoft.Json;
using SQLite;

namespace iPadPos
{
	public class InvoiceLine : BaseModel
	{
		[PrimaryKey, AutoIncrement]
		public int LocalId { get; set; }
		public int LocalParentId { get; set;}
		public InvoiceLine ()
		{
		}
		public InvoiceLine(Item item)
		{
			Qty = 1;
			Description = item.Description;
			Cost = item.Cost;
			TaxCode = item.TaxCode;
			Price = item.Price;
			TransactionCode = item.TransCode;
			ItemId = item.ItemID;
			OnHand = item.OnHand;
			ItemType = item.ItemType;
			var cpn = item as Coupon;
			if (cpn != null) {
				DiscountPercent = cpn.DiscountPercent;
				CouponSelectedOnly = cpn.SelectedItemsOnly;
				CouponIsValid = cpn.IsValidToday;
			}
		}

		public bool CouponIsValid { get; set; }

		[JsonIgnore]
		public ItemType ItemType {get;set;}
		public int ParentRecordId { get; set; }

		string itemId;
		public string ItemId {
			get {
				return itemId;
			}
			set { ProcPropertyChanged (ref itemId, value); }
		}

		string transactionCode;
		[JsonProperty("TransCode")]
		public string TransactionCode {
			get {
				return transactionCode;
			}
			set { 
				if (ProcPropertyChanged (ref transactionCode, value) && TransType == null || TransType.Id != transactionCode)
					TransType = Database.Main.Table<TransactionType> ().Where (x => x.Id == transactionCode).FirstOrDefault ();

			}
		}
		TransactionType transType;
		[JsonIgnore, SQLite.Ignore]
		public TransactionType TransType {
			get {
				return transType;
			}
			set {
				transType = value;
				if (transType == null)
					return;
				TransactionCode = TransType.Id;
				Qty = Math.Abs (Qty) * TransType.Multiplier;
			}
		}

		int qty;
		public int Qty {
			get {
				return qty;
			}
			set { 
				if(ProcPropertyChanged (ref qty, value))
				updateTotals();
			}
		}

		double discount;
		public double Discount {
			get {
				return discount;
			}
			set { 
				if (ProcPropertyChanged (ref discount, value)) {
					updateTotals ();
					ProcPropertyChanged ("DiscountString");
				}
			}
		}

		bool selected;
		public bool Selected {
			get {
				return selected;
			}
			set { ProcPropertyChanged (ref selected, value); }
		}
		public bool CouponSelectedOnly {get;set;}
		public float DiscountPercent { get; set; }
		public string DiscountString {
			get {
				return Discount.ToString("C");
			}
//			set {
//				discountString = value;
//			}
		}

		string description;
		public string Description {
			get {
				return description;
			}
			set { ProcPropertyChanged (ref description, value); }
		}

		double price;
		public double Price {
			get {
				return price;
			}
			set { 
				if (ProcPropertyChanged (ref price, value)) {
					updateTotals ();
					ProcPropertyChanged ("PriceString");
				}
			}
		}

		string taxCode;
		public string TaxCode {
			get {
				return taxCode;
			}
			set { ProcPropertyChanged (ref taxCode, value); }
		}

		double cost;
		public double Cost {
			get {
				return cost;
			}
			set { ProcPropertyChanged (ref cost, value); }
		}

		int onHand;
		public int OnHand {
			get {
				return onHand;
			}
			set { ProcPropertyChanged (ref onHand, value); }
		}

		double finalPrice;
		public double FinalPrice {
			get {
				return finalPrice;
			}
			set { 
				if (ProcPropertyChanged (ref finalPrice, Math.Round(value,2)))
					ProcPropertyChanged ("FinalPriceString");
			}
		}
		double subTotal;
		public double SubTotal {
			get{ return subTotal; }
			set{
				ProcPropertyChanged (ref subTotal, Math.Round (value, 2));
			}
		}
		void updateTotals()
		{
			SubTotal = Math.Round(Qty * price,2);
			FinalPrice = Math.Round(Qty *  (Price - Discount),2);
		}

		public string PriceString
		{
			get{return Price.ToString("C");}
		}

		public string FinalPriceString
		{
			get{return FinalPrice.ToString("C");}
		}

		public void ToggleSelected()
		{
			if (ItemType == ItemType.Coupon || ItemType == ItemType.NewCustomerTracking)
				return;
			Selected = !Selected;
		}
	}
}

