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
			BackgroundGray = Color.LightGray;//UIColor.FromRGB(196,196,196);
			SideBarCellColor = Color.DarkGray;//UIColor.FromRGB (54, 54, 54);
			SideBarBackGroundColor = Color.Gray;
			PayColor = Color.Olive;//UIColor.FromRGB (167, 204, 27);

		}

		public UIColor BackgroundGray{get;set;}
		public UIColor SideBarCellColor {get;set;}
		public UIColor PayColor{get;set;}
		public UIColor SideBarBackGroundColor { get; set; }
		public UIColor FormLabelColor = UIColor.FromRGB (100, 100, 100);
		public Lazy<UIFont> FormLabelFont = new Lazy<UIFont>(() => UIFont.SystemFontOfSize(UIFont.SmallSystemFontSize));
	}
}

