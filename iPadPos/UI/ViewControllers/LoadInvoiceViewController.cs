using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class LoadInvoiceViewController : UIViewController
	{
		public Action<Invoice> InvoiceSelected {get;set;}
		readonly ObservableTableView TableView;
		public LoadInvoiceViewController ()
		{
			Title = "Load Invoice";
			NavigationItem.RightBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Refresh,(s,e) => ReloadData());
			View = TableView = new ObservableTableView {
				CellIdentifier = InvoiceCell.Key,
				CreateCellFunc = () => new InvoiceCell (),
				BindCellAction = (cell,obj) => (cell as InvoiceCell).Invoice = obj as Invoice,
				ItemTapped = (i) =>{
					if(InvoiceSelected != null)
						InvoiceSelected(i as Invoice);
				},
			};

		}
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			ReloadData ();
		}

		async void ReloadData()
		{
			TableView.DataSource = await WebService.Main.GetInvoices();
		}
	}
}

