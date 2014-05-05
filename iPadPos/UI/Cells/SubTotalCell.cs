using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class SubTotalCell : UITableViewCell
	{
		public SubTotalCell () : base (UITableViewCellStyle.Value1,"subtotalcell")
		{
			TextLabel.Text = "Subtotal";
			SelectionStyle = UITableViewCellSelectionStyle.None;
			BackgroundColor = Theme.Current.SideBackgroundColor;
			TextLabel.TextColor = UIColor.White;
			DetailTextLabel.TextColor = UIColor.White;
		}
	}
}

