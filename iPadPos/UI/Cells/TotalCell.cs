using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class TotalCell : UITableViewCell
	{
		public TotalCell () : base (UITableViewCellStyle.Value1,"totalcell")
		{
			//TextLabel.Text = "Total";
			SeparatorInset = new UIEdgeInsets (0, 0, 0, 0);
			SelectionStyle = UITableViewCellSelectionStyle.None;
			TextColor = UIColor.White;
			BackgroundColor = Theme.Current.SideBackgroundColor;
			DetailTextLabel.Font = UIFont.BoldSystemFontOfSize (30);
		}
		double value;
		public double Value {
			get {
				return value;
			}
			set {
				this.value = value;
				update ();
			}
		}

		UIColor textColor;
		public UIColor TextColor {
			get {
				return textColor;
			}
			set {
				textColor = value;
				update ();
			}
		}

		void update()
		{
			DetailTextLabel.Text = Value.ToString ("C");
			DetailTextLabel.TextColor = Value >= 0 ? TextColor : UIColor.Red;
		}
	}
}

