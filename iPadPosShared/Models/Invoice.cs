using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;

namespace iPadPos
{
	public class Invoice : BaseModel
	{
		public Invoice()
		{
			Customer = new Customer ();
			Items = new ObservableCollection<InvoiceLine> ();
			Payments = new List<Payment> ();
		}
		public int RecordId {get;set;}

		[JsonProperty("InvoiceID")]
		public int Id {get;set;}

		public Customer Customer {get;set;}

		[JsonProperty("InvDate")]
		public DateTime Date {get;set;}

		public string RegisterId { get; set; }

		public List<Payment> Payments {get;set;}

		ObservableCollection<InvoiceLine> items;
		[JsonProperty("Lines")]
		public ObservableCollection<InvoiceLine> Items {
			get {
				return items;
			}
			set {
				unbindAll ();
				items = value;
				Items.CollectionChanged += HandleCollectionChanged;
				bindAll ();
			}
		}

		public void AddItem(Item item)
		{
			Items.Add (new InvoiceLine (item));
		}

		void HandleCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			UpdateTotals ();
		}
		void bindAll()
		{
			if (Items == null)
				return;
			foreach(var item in Items.OfType<INotifyPropertyChanged>().ToList())
			{
				item.PropertyChanged += HandlePropertyChanged;
			}
		}
		void unbindAll()
		{
			if (Items == null)
				return;
			foreach(var item in Items.OfType<INotifyPropertyChanged>().ToList())
			{
				item.PropertyChanged -= HandlePropertyChanged;
			}
		}

		void HandlePropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "FinalPrice")
				UpdateTotals ();
		}

		double discountAmount;
		public double DiscountAmount {
			get {
				return discountAmount;
			}
			set { ProcPropertyChanged (ref discountAmount, value); }
		}
		public double TaxAmount {get;set;}
		double subTotal;
		public double SubTotal {
			get {
				return subTotal;
			}
			set { 
				if(ProcPropertyChanged (ref subTotal, value))
					ProcPropertyChanged("SubtotalString");
			}
		}

		double total;
		public double Total {
			get {
				return total;
			}
			set { 
				if(ProcPropertyChanged (ref total, value))
					ProcPropertyChanged("TotalString");
			}
		}

		public string TotalString
		{
			get{ return Total.ToString ("C"); }
		}

		public string SubtotalString
		{
			get{ return SubTotal.ToString ("C"); }
		}

		void UpdateTotals()
		{
			SubTotal = Items.Sum (x => x.FinalPrice);

			Total = SubTotal - DiscountAmount;
		}
	}
}

