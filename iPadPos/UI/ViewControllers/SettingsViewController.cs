using System;
using MonoTouch.Dialog;

namespace iPadPos
{
	public class SettingsViewController : DialogViewController
	{
		public SettingsViewController () : base(MonoTouch.UIKit.UITableViewStyle.Grouped,null)
		{
			Root = new RootElement ("Settings") {
				new Section("Server Settings"){

				},
				new Section("iPad Settings"){

				},
				new Section("Payment Settings"){

				},
			};
		}
	}
}

