using System;
using MonoTouch.UIKit;
using iOSHelpers;

namespace iPadPos
{
	public class InvoiceHeaderCell : ColumnCell
	{
		public const string Key = "InvoiceLineCell";
		public InvoiceHeaderCell () : base (Key)
		{
			UIColor headerColor = UIColor.Black;
			Padding = 5;
			BackgroundView = new UIBlurView{
				//TintColor = Theme.Current.PayColor,
				//AccentColorIntensity = .1f,
			};
			BackgroundColor = UIColor.Clear;
			//BackgroundColor = UIColor.LightGray.ColorWithAlpha (.25f);
			AutoAddSubview (Description = new UILabel {
				Text = "Description",
				TextAlignment = UITextAlignment.Left,
				TextColor = headerColor
			},4);
				

			AutoAddSubview (Price = new UILabel {
				Text = "Price",
				TextAlignment = UITextAlignment.Center,
				TextColor = headerColor
			},2);

			AutoAddSubview (Discount = new UILabel{ 
				Text = "Discount", 
				TextAlignment = UITextAlignment.Center,
				TextColor = headerColor
			}, 2);
			AutoAddSubview (new UILabel{ 
				Text = "T", 
				TextAlignment = UITextAlignment.Center,
				TextColor = headerColor
			});
			AddSubview (Total = new UILabel{ 
				Text = "Total", 
				TextAlignment = UITextAlignment.Center,
				TextColor = headerColor
			},9,2);
		}

		UILabel Description;
		UILabel Qty;
		UILabel Price;
		UILabel Subtotal;
		UILabel Discount;
		UILabel Total;
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			//BackgroundView.Frame = Bounds;
		}



	

	}
}

