using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class LoadInvoiceViewController : UIViewController
	{
		ObservableTableView TableView;
		public LoadInvoiceViewController ()
		{
			Title = "Load Invoice";
			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Refresh,(s,e) => ReloadData());
			View = TableView = new ObservableTableView ();

		}

		async void ReloadData()
		{
			TableView.DataSource = await WebService.Main.GetInvoices();
		}
	}
}

