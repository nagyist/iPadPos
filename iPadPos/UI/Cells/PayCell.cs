using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class PayCell : ButtonCell
	{
		public Action Tap { get; set; }
		public PayCell () : base ("paycell")
		{
			BackgroundColor = UIColor.FromRGB (38, 143, 130);
		}
		public override void Tapped ()
		{
			base.Tapped ();
			Tap ();
		}
	}
}

