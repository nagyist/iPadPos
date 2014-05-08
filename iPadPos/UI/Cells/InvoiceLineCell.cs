using System;
using MonoTouch.UIKit;
using Praeclarum.Bind;
using iOSHelpers;
using System.Linq;

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
			TransTypeButton.TouchUpInside += (sender, e) => {
				var sheet = new SimpleActionSheet();
				var types = Database.Main.Table<TransactionType>().ToList();
				types.ForEach(x=> sheet.Add(x.Description,()=> Line.TransType = x));
				sheet.ShowFrom(TransTypeButton.Bounds,TransTypeButton,true);
			};
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
				Price.Text == Line.PriceString &&
				Total.Text == Line.FinalPriceString &&
				TransTypeButton.Title == line.TransactionCode
			);
			line.SubscribeToProperty ("FinalPrice", updateTextColor);
		}

		void Unbind ()
		{
			if (binding == null)
				return;
			binding.Unbind ();
			line.UnSubscribeToProperty ("FinalPrice",updateTextColor);
		}

		void updateTextColor()
		{
			Total.TextColor = (line.FinalPrice < 0 ? UIColor.Red : UIColor.Black);
		}

	}
}

