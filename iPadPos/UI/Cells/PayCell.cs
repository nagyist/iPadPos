using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class PayCell : ButtonCell
	{
		public PayCell () : base ("paycell")
		{
			BackgroundColor = Theme.Current.PayColor;
			//BackgroundColor = UIColor.FromRGB (38, 143, 130);
			this.TextLabel.TextColor = UIColor.White;
			this.TextLabel.Font = UIFont.BoldSystemFontOfSize (20);
		}
	}
}

