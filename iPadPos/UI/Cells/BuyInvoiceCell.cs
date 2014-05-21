using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class BuyInvoiceCell : ColumnCell
	{
		public const string Key = "BuyInvoiceCell";
		UILabel Name;
		UILabel Type;
		UILabel Amount;
		public BuyInvoiceCell () : base(Key)
		{
			this.AutoAddSubview (Name = new UILabel{Text = "Name"}, 7);
			Name.SizeToFit ();
			this.AutoAddSubview (Type = new UILabel {Text = "Cash", TextColor = Color.Olive}, 2);
			Type.SizeToFit ();
			this.AutoAddSubview (Amount = new UILabel {Text = "$100.00",TextColor = Color.Red, TextAlignment = UITextAlignment.Right}, 2);
			Amount.SizeToFit ();
		}
		BuyInvoice invoice;
		public BuyInvoice Invoice {
			get {
				return invoice;
			}
			set {
				invoice = value;
				bind ();
			}
		}
		void bind()
		{
			Name.Text = invoice.CustomerName;
			Type.Text = invoice.IsOnAccount ? "On Account" : "Cash";
			Amount.Text = Math.Abs(invoice.Total).ToString("C");

		}
	}
}

