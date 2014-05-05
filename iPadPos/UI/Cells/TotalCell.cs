using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class TotalCell : UITableViewCell
	{
		public TotalCell () : base (UITableViewCellStyle.Value1,"totalcell")
		{
			//TextLabel.Text = "Total";
			SelectionStyle = UITableViewCellSelectionStyle.None;

			BackgroundColor = Theme.Current.SideBackgroundColor;
			DetailTextLabel.Font = UIFont.BoldSystemFontOfSize (30);
			DetailTextLabel.TextColor = UIColor.White;
		}
	}
}

