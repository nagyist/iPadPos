using System;
using MonoTouch.UIKit;
using FlyoutNavigation;
using MonoTouch.Foundation;
using MonoTouch.Dialog;

namespace iPadPos
{
	public static class App
	{
//		static FlyoutNavigationController flyout;
		public static UIViewController Create()
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
				return new UINavigationController(new InvoiceViewController ()){
					NavigationBar = {
						BarStyle = UIBarStyle.BlackTranslucent,
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

