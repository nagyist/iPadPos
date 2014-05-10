﻿using System;
using Newtonsoft.Json;

namespace iPadPos
{
	public class InvoiceLine : BaseModel
	{
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
		}

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

		int taxCode;
		public int TaxCode {
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
		void updateTotals()
		{
			FinalPrice = Qty * Price;
		}

		public string PriceString
		{
			get{return Price.ToString("C");}
		}

		public string FinalPriceString
		{
			get{return FinalPrice.ToString("C");}
		}
	}
}

