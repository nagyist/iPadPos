using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class PrintLastInvoiceCell : ButtonCell
	{
		public PrintLastInvoiceCell () : base ("PrintLastInvoiceCell")
		{
			BackgroundColor = Color.Orange;
			//BackgroundColor = UIColor.FromRGB (38, 143, 130);
			this.TextLabel.TextColor = UIColor.White;
			this.TextLabel.Font = UIFont.BoldSystemFontOfSize (20);
		}
	}
}

