﻿using System;
using MonoTouch.UIKit;
using Praeclarum.Bind;
using iOSHelpers;
using System.Linq;

namespace iPadPos
{
	public class InvoiceLineCell : ColumnCell, ICellSelectable
	{
		public const string Key = "InvoiceLineCell";
		UIPopoverController popup;
		public InvoiceLineCell () : base (Key)
		{
			AutoAddSubview (Description = new UILabel {
				Text = "Description",
			}, 4
			);

			AutoAddSubview (Price = new UILabel{ Text = "Price",TextAlignment = UITextAlignment.Center },2);
			AutoAddSubview (Discount = new UIBorderedButton () {
				Title = "0",
				Tapped = (b) =>{

					if(popup != null)
						popup.Dispose();

					var d = new DiscountViewController(line.Price){
						DollarChanged = (dollar) =>{
							popup.Dismiss(true);
							Line.Discount = dollar;
						}
					};

					popup = new UIPopoverController(d);
					popup.DidDismiss += (object sender, EventArgs e) => {
						line.Discount = 0;
						d.Dispose();
						popup.Dispose();
						popup = null;
					};
					popup.PresentFromRect(Discount.Bounds,Discount, UIPopoverArrowDirection.Any,true);
				}},2);

			AutoAddSubview (TransTypeButton = new UIBorderedButton{ Title = "S" ,TintColor = Color.LightBlue});
			TransTypeButton.TouchUpInside += (sender, e) => {
				var sheet = new SimpleActionSheet();
				var types = Database.Main.Table<TransactionType>().ToList();
				types.ForEach(x=> sheet.Add(x.Description,Color.LightBlue,()=> Line.TransType = x));
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
				updateTextColor ();
				updateSelected ();
			}
		}

		Binding binding;

		void Bind ()
		{
			binding = Binding.Create (() =>
				Description.Text == Line.Description &&
				Price.Text == Line.PriceString &&
				Total.Text == Line.FinalPriceString &&
				TransTypeButton.Title == line.TransactionCode && 
				Discount.Title == line.DiscountString
			);
			line.SubscribeToProperty ("FinalPrice", updateTextColor);
			line.SubscribeToProperty ("Selected", updateSelected);
		}

		void Unbind ()
		{
			if (binding == null)
				return;
			binding.Unbind ();
			line.UnSubscribeToProperty ("FinalPrice",updateTextColor);
			line.UnSubscribeToProperty ("Selected",updateSelected);
		}

		void updateTextColor()
		{
			Total.TextColor = (line.FinalPrice < 0 ? Color.Red : UIColor.Black);
			Discount.TintColor = (line.Discount == 0) ? Color.Gray : (UIColor)Color.Red;
		}
		void updateSelected()
		{
			Accessory = line.Selected ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
		}
		

		#region ICellSelectable implementation
		public void OnSelect ()
		{
			Line.ToggleSelected ();
		}
		#endregion
	}
}

