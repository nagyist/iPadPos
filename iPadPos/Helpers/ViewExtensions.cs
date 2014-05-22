using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace iPadPos
{
	public static class ViewExtensions
	{
		public static void DismissKeyboard(this UIView view)
		{
			var tv = new UITextField (new RectangleF(-100,-100,1,1));
			view.AddSubview (tv);
			tv.BecomeFirstResponder ();
			tv.ResignFirstResponder ();
			tv.RemoveFromSuperview ();
			tv.Dispose ();
		}
	}
}

