using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class DiscountCell : UITableViewCell
	{
		public DiscountCell () : base (UITableViewCellStyle.Value1,"discountcell")
		{
			TextLabel.Text = "Discount";

			TextLabel.TextColor = UIColor.White;
			DetailTextLabel.TextColor = UIColor.Red;
			BackgroundColor = Theme.Current.SideBackgroundColor;
		}
	}
}

