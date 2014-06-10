using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class LastPostedCell : UITableViewCell
	{
		public LastPostedCell () : base (UITableViewCellStyle.Value1,"LastPostedCell")
		{
			TextLabel.Text = "Last Change";
			SelectionStyle = UITableViewCellSelectionStyle.None;
			BackgroundColor = Theme.Current.SideBarCellColor;
			TextLabel.TextColor = UIColor.White;
			DetailTextLabel.TextColor = Color.Orange;
			DetailTextLabel.Font = UIFont.BoldSystemFontOfSize (20);
		}
	}
}

