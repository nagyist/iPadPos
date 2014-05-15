using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class NewCustomerInfoViewController : UIViewController
	{
		ObservableTableView TableView
		{
			get{ return View as ObservableTableView; }
		}
		public UIPopoverController Popover { get; set;}
		public Action<Item> HowTheyHeard { get; set;}
		public NewCustomerInfoViewController ()
		{
			Title = "How did they hear about us?";
			TableView.BindCellAction = (cell, item) => {
				cell.TextLabel.Text = (item as Item).Description;
			};
			TableView.ItemTapped = (obj) => {
				if(HowTheyHeard != null)
					HowTheyHeard(obj as Item);
			};
		}
		public override void LoadView ()
		{
			View = new ObservableTableView ();
		}
		public override async void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			if (Popover != null)
				Popover.ShouldDismiss = shouldDismiss;
			TableView.DataSource = await WebService.Main.GetNewCustomerInformation();
		}
		bool shouldDismiss(UIPopoverController pop)
		{
			return false;
		}
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			Popover.ShouldDismiss = null;
		}
	}
}

