using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace iPadPos
{
	public class CellTableViewSource : UITableViewSource
	{
		UITableViewCell[] tableItems;

		public CellTableViewSource (UITableViewCell[] items)
		{
			tableItems = items;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return tableItems.Length;
		}

		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			return tableItems [indexPath.Row];
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return tableItems [indexPath.Row].Frame.Height;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (tableItems [indexPath.Row] is ICellSelectable)
				((ICellSelectable)tableItems [indexPath.Row]).Tapped ();

			tableView.DeselectRow (indexPath, true);
		}
		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			Console.WriteLine ("Disposing : {0}", disposing);
		}

	}
}