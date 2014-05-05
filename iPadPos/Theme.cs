using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class Theme
	{
		static Theme current;
		public static Theme Current {
			get {
				return current ?? (new Theme());
			}
			set {
				current = value;
			}
		}

		public Theme()
		{
			BackgroundGray = UIColor.FromRGB(196,196,196);
			SideBackgroundColor = UIColor.FromRGB (70, 70, 70);
		}

		public UIColor BackgroundGray{get;set;}
		public UIColor SideBackgroundColor {get;set;}
	}
}

