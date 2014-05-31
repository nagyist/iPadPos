using System;
using MonoTouch.UIKit;
using FlyoutNavigation;
using MonoTouch.Foundation;
using MonoTouch.Dialog;

namespace iPadPos
{
	public static class App
	{
		public static InvoiceViewController InvoiceViewController;
//		static FlyoutNavigationController flyout;
		public static UIViewController Create ()
		{
//			flyout = new FlyoutNavigationController () {
//				ForceMenuOpen = false,
//				AlwaysShowLandscapeMenu = false,
//				NavigationRoot = new RootElement("")
//				{
//					new Section(){
//						new StringElement("Sales"),
//					},
//
//				}
//			};
//			flyout.ViewControllers = new UIViewController[] {
			return new UINavigationController (InvoiceViewController = new InvoiceViewController ()) {
				NavigationBar = {
					BarStyle = UIBarStyle.Black,
					Translucent = true,
				}
			};
//				,
//			};
//			return flyout;

		}
//		public static void ToggleMenu()
//		{
//			flyout.ToggleMenu ();
//		}

		public static void ShowAlert (string title, string message)
		{
			EnsureRunOnMainThread (() => new UIAlertView (title, message, null, "Ok").Show ());
		}

		static NSObject invoker;

		public static void EnsureRunOnMainThread (Action action)
		{
			if (NSThread.Current.IsMainThread) {
				action ();
				return;
			}
			if (invoker == null)
				invoker = new NSObject ();
			invoker.BeginInvokeOnMainThread (() => action ());
		}

		public static bool CanOpenUrl(string url)
		{
			return UIApplication.SharedApplication.CanOpenUrl (new NSUrl(url));
		}

		public static bool OpenUrl (string url)
		{
			return UIApplication.SharedApplication.OpenUrl (new NSUrl(url));
		}

		public static bool HasPaypal()
		{
			return CanOpenUrl("paypalhere://takePayment");
		}
	}
}

