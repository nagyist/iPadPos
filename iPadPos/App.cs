using System;
using MonoTouch.UIKit;
using FlyoutNavigation;
using MonoTouch.Foundation;

namespace iPadPos
{
	public static class App
	{
		public static UIViewController Create()
		{
//			var flyout = new FlyoutNavigationController () {
//				NavigationRoot = ne
//			};

			return new UINavigationController(new InvoiceViewController ()){
				NavigationBar = {
					BarStyle = UIBarStyle.BlackTranslucent,
				}
			};
		}

		public static void ShowAlert(string title, string message)
		{
			EnsureRunOnMainThread (() => new UIAlertView (title, message, null, "Ok").Show ());
		}
		static NSObject invoker;
		public static void EnsureRunOnMainThread(Action action)
		{
			if (NSThread.Current.IsMainThread) {
				action ();
				return;
			}
			if (invoker == null)
				invoker = new NSObject ();
			invoker.BeginInvokeOnMainThread (() => action());
		}
	}
}

