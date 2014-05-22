using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;
using System.Threading.Tasks;

namespace iPadPos
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;

		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			ApplyTheme ();
			window.RootViewController = App.Create ();
			window.TintColor = Theme.Current.PayColor;
			window.MakeKeyAndVisible ();
			SyncAll ();
			return true;
		}

		void ApplyTheme()
		{

		}

		async Task SyncAll(bool showSpinner = false)
		{
			try{
				if(showSpinner)
					BigTed.BTProgressHUD.ShowContinuousProgress();
				await WebService.Main.SyncAll ();
				return;
			}
			catch(Exception ex) {
				Console.WriteLine (ex);
			}
			finally{
				if (showSpinner)
					BigTed.BTProgressHUD.Dismiss ();
			}
			var alert = new UIAlertView ("Error", "There was an error connecting to the server", null, "Try Again","Settings");
			alert.Clicked += async (object sender, UIButtonEventArgs e) => {
				if(e.ButtonIndex == 1)
				{
					var settings = new SettingsViewController();
					await window.RootViewController.PresentViewControllerAsync(new UINavigationController(settings),true);
					await settings.Saved();
				}
				await SyncAll(true);
			};
			alert.Show ();

		}
		public override void WillEnterForeground (UIApplication application)
		{
			App.InvoiceViewController.SetTitle ();
		}


	}
}

