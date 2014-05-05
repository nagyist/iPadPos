using System;
using MonoTouch.UIKit;
using Praeclarum.Bind;

namespace iPadPos
{
	public class InvoiceLineCell : ColumnCell
	{
		public const string Key = "InvoiceLineCell";

		public InvoiceLineCell () : base (Key)
		{
			AutoAddSubview (Description = new UILabel {
				Text = "Description",
			}, 4
			);

			AutoAddSubview (Price = new UILabel{ Text = "Price",TextAlignment = UITextAlignment.Center },2);
			AutoAddSubview (Discount = new UIBorderedButton () {
				Title = "0",
			},2);
			AutoAddSubview (TransTypeButton = new UIBorderedButton{ Title = "S" });
			AddSubview (Total = new UILabel{ Text = "Total",TextAlignment = UITextAlignment.Center },9,columnspan:2);
		}

		UILabel Description;
		UILabel Price;
		UIBorderedButton Discount;
		UILabel Total;
		UIBorderedButton TransTypeButton;
	




		InvoiceLine line;

		public InvoiceLine Line {
			get {
				return line;
			}
			set {
				Unbind ();
				line = value;
				Bind ();
			}
		}

		Binding binding;

		void Bind ()
		{
			binding = Binding.Create (() =>
				Description.Text == Line.Description &&
				Price.Text == Line.FinalPrice.ToString ("C") &&
				Total.Text == Line.FinalPrice.ToString("C") &&
				TransTypeButton.Title == line.TransactionCode 
			);
		}

		void Unbind ()
		{
			if (binding == null)
				return;
			binding.Unbind ();
		}

	}
}

