using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Collections.Specialized;
using SQLite;

namespace iPadPos
{
	public class Invoice : BaseModel
	{
		int localId;
		[PrimaryKey, AutoIncrement]
		public int LocalId {
			get {
				return localId;
			}
			set {
				localId = value;
				if (Items != null)
					Items.ForEach (x => x.LocalParentId = LocalId);
			}
		}
		public Invoice()
		{
			Customer = new Customer ();
			Payments = new ObservableCollection<Payment> ();
			Items = new ObservableCollection<InvoiceLine> ();
		}
		public int RecordId {get;set;}

		[JsonProperty("InvoiceID")]
		public string Id {get;set;}

		Customer customer;
		[SQLite.Ignore]
		public Customer Customer {
			get {
				return customer;
			}
			set { 
				ProcPropertyChanged (ref customer, value);
			}
		}

		[JsonProperty("InvDate")]
		public DateTime Date {get;set;}

		public string RegisterId { get; set; }

		ObservableCollection<Payment> payments;
		[SQLite.Ignore]
		public ObservableCollection<Payment> Payments {
			get {
				return payments;
			}
			set {
				unbindAllPayments ();
				payments = value;
				payments.CollectionChanged += HandlePaymentsCollectionChanged;
				bindAllPayments ();
			}
		}
		void HandlePaymentsCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			UpdateTotals ();
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
				e.OldItems.Cast<Payment> ().ToList ().ForEach (x => x.PropertyChanged -= HandlePropertyChanged);
			else if (e.Action == NotifyCollectionChangedAction.Add) {
				e.NewItems.Cast<Payment> ().ToList ().ForEach (x => x.PropertyChanged += HandlePropertyChanged);
			}
		}
		void bindAllPayments()
		{
			if (Payments == null)
				return;
			foreach(var item in Payments.OfType<INotifyPropertyChanged>().ToList())
			{
				item.PropertyChanged += HandlePropertyChanged;
			}
		}
		void unbindAllPayments()
		{
			if (Payments == null)
				return;

			payments.CollectionChanged -= HandlePaymentsCollectionChanged;
			
			foreach(var item in Payments.OfType<INotifyPropertyChanged>().ToList())
			{
				item.PropertyChanged -= HandlePropertyChanged;
			}
		}

		void HandlePropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (sender is InvoiceLine)
				Database.Main.Update (sender);
			if (e.PropertyName == "FinalPrice" || e.PropertyName == "Amount")
				UpdateTotals ();
		}

		/// <summary>
		/// The items.
		/// </summary>

		ObservableCollection<InvoiceLine> items;
		[JsonProperty("Lines"), SQLite.Ignore]
		public ObservableCollection<InvoiceLine> Items {
			get {
				return items;
			}
			set {
				unbindAllItems ();
				items = value;
				UpdateTotals ();
				Items.CollectionChanged += HandleItemsCollectionChanged;
				bindAllItems ();
			}
		}

		public void AddItem(Item item)
		{
			var i = new InvoiceLine (item);
			i.LocalParentId = LocalId;
			i.PropertyChanged += HandlePropertyChanged;
			Items.Add (i);
			Database.Main.Insert (i);
		}

		void HandleItemsCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			UpdateTotals ();
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
				e.OldItems.Cast<InvoiceLine> ().ToList ().ForEach (x => {
					x.PropertyChanged -= HandlePropertyChanged;
					if(x is InvoiceLine)
						Database.Main.Delete(x);
				});

			}
		}
		void bindAllItems()
		{
			if (Items == null)
				return;
			foreach(var item in Items.OfType<INotifyPropertyChanged>().ToList())
			{
				item.PropertyChanged += HandlePropertyChanged;
			}
		}
		void unbindAllItems()
		{
			if (Items == null)
				return;
			Items.CollectionChanged -= HandleItemsCollectionChanged;
			foreach(var item in Items.OfType<INotifyPropertyChanged>().ToList())
			{
				item.PropertyChanged -= HandlePropertyChanged;
			}
		}


		double discountAmount;
		public double DiscountAmount {
			get {
				return discountAmount;
			}
			set { 
				if(ProcPropertyChanged (ref discountAmount, Math.Round(value,2)))
					ProcPropertyChanged("DiscountString");
			}
		}
		public double TaxAmount {get;set;}
		double subTotal;
		public double SubTotal {
			get {
				return subTotal;
			}
			set { 
				ProcPropertyChanged (ref subTotal, Math.Round (value, 2));
			}
		}

		double total;
		public double Total {
			get {
				return total;
			}
			set { 
				if(ProcPropertyChanged (ref total, Math.Round(value,2)))
					ProcPropertyChanged("TotalString");
			}
		}

		public string TotalString
		{
			get{ return Total.ToString ("C"); }
		}

		public string ItemsSubtotalString
		{
			get{ return ItemsSubtotal.ToString ("C"); }
		}

		public string DiscountString {
			get { return (DiscountAmount * -1 ).ToString("C"); }
		}

		public double TotalDiscount {
			get{ return discountAmount + couponDiscount; }
		}
		public string TotalDiscountString
		{
			get{ 
				return TotalDiscount.ToString ("C");
			}
		}
		double couponDiscount =0;
		[JsonIgnore,Ignore]
		public List<InvoiceLine> Coupons
		{
			get{
				return Items.Where(x=> x.ItemType == ItemType.Coupon).ToList ();
			}
		}
		void updateCoupons()
		{
			Coupons.Where (x => x.DiscountPercent > 0).ForEach (x => {
				x.Price = ItemsSubtotal * x.DiscountPercent * -1f;
			});
			couponDiscount = Coupons.Sum (x => x.Price);
			ProcPropertyChanged("TotalDiscountString");
		}

		double itemSubtotal;
		public double ItemsSubtotal
		{
			get{ return itemSubtotal; }
			set { 
				if(ProcPropertyChanged (ref itemSubtotal, Math.Round(value,2)))
					ProcPropertyChanged("ItemsSubtotalString");
			}

		}
		void UpdateTotals()
		{
			ItemsSubtotal = Items.Where(x => x.ItemType != ItemType.Coupon).Sum (x => x.SubTotal);
			updateCoupons ();
			SubTotal = Items.Sum (x => x.FinalPrice);
			Total = SubTotal - DiscountAmount;
			ProcPropertyChanged("DiscountString");
			ProcPropertyChanged("Discount");

			AppliedPayment = Payments.Sum (x => x.Amount);
			Remaining = AppliedPayment >= Total && Total > 0 ? 0 : Total - AppliedPayment;
			Change = AppliedPayment <= Total ? 0 : AppliedPayment - Total;
			if(Items.Count >= 0)
				Save ();
		}

		double appliedPayment;
		public double AppliedPayment {
			get {
				return appliedPayment;
			}
			set {
				if(ProcPropertyChanged (ref appliedPayment, Math.Round(value,2)))
					ProcPropertyChanged("AppliedPaymentString");
			}
		}

		double remaining;
		public double Remaining {
			get {
				return remaining;
			}
			set {
				if(ProcPropertyChanged (ref remaining, Math.Round(value,2)))
					ProcPropertyChanged("RemainingString");
			}
		}

		double change;
		public double Change {
			get {
				return change;
			}
			set {
				if(ProcPropertyChanged (ref change, Math.Round(value,2)))
					ProcPropertyChanged("ChangeString");
				if(CashPayment != null)
				CashPayment.Change = value;
			}
		}

		public string AppliedPaymentString
		{
			get{ return AppliedPayment.ToString ("C"); }
		}

		public string ChangeString
		{
			get{ return Change.ToString ("C"); }
		}


		public string RemainingString
		{
			get{ return Remaining.ToString ("C"); }
		}

		[Newtonsoft.Json.JsonIgnore]
		public Payment CashPayment
		{
			get { return Payments.Where (x => x.PaymentType.Id == "Cash").FirstOrDefault (); }
		}
			
		public Tuple<bool,string> IsReadyForPayment ()
		{
			if (Customer == null || string.IsNullOrEmpty(Customer.CustomerId))
				return new Tuple<bool, string> (false, "Invoice requires a customer");

			if (!HasItems ())
				return new Tuple<bool,string> (false, "There are no items on this invoice");

			return new Tuple<bool, string> (true, "Ready to go");
		}
		public bool HasItems()
		{
			return items.Any ();
		}

		public Tuple<bool,string> Validate()
		{
			var result = IsReadyForPayment ();
			if (!result.Item1)
				return result;

			if(remaining != 0)
				return new Tuple<bool,string>(false, "There is still a remaining balance");

			return new Tuple<bool, string> (true, "Ready to go");
		}

		public void PaymentSelected (Payment payment)
		{
			if (payment.PaymentType.Id == "Acct") {
				payment.Amount = Math.Min (Remaining, Customer.OnAccount);
				return;
			}
			payment.Amount = Remaining;
		}
		bool hasSaved = false;
		public void Save()
		{
			if (LocalId == 0)
				Database.Main.Insert (this);
			else
				Database.Main.Update (this);
			if (hasSaved)
				return;
			Database.Main.InsertAll (Items, "OR REPLACE");

			hasSaved = true;
		}
		public void Save(bool force)
		{
			if (force)
				hasSaved = false;
		}

		public void DeleteLocal()
		{
			Items.ForEach (x => Database.Main.Delete (x));
			Database.Main.Delete (this);
			Settings.Shared.CurrentInvoice = 0;
		}
		public static Invoice FromLocalId(int id)
		{
			var invoice = Database.Main.Table<Invoice> ().Where (x => x.LocalId == id).FirstOrDefault () ?? new Invoice ();
			if (invoice.LocalId != 0) {
				invoice.Items = new  ObservableCollection<InvoiceLine> (Database.Main.Table<InvoiceLine> ());
			}
			return invoice;
		}
	}
}

