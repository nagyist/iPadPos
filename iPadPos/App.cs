using System;
using MonoTouch.UIKit;
using FlyoutNavigation;

namespace iPadPos
{
	public static class App
	{
		public static UIViewController Create()
		{
//			var flyout = new FlyoutNavigationController () {
//				NavigationRoot = ne
//			};

			return new UINavigationController(new InvoiceViewController ());
		}
	}
}

