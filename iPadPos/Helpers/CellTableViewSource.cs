using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace iPadPos
{
	public class CellTableViewSource : UITableViewSource, IList<UITableViewCell>
	{
		readonly List<UITableViewCell> items = new List<UITableViewCell>();
		UITableView tableView;
		public CellTableViewSource ()
		{

		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			this.tableView = tableView;
			return items.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			this.tableView = tableView;
			return items [indexPath.Row];
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			this.tableView = tableView;
			return items [indexPath.Row].Frame.Height;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			this.tableView = tableView;
			if (items [indexPath.Row] is ICellSelectable)
				((ICellSelectable)items [indexPath.Row]).OnSelect ();

			tableView.DeselectRow (indexPath, true);
		}
		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			Console.WriteLine ("Disposing : {0}", disposing);
		}

		#region IList implementation
		public int IndexOf (UITableViewCell item)
		{
			return items.IndexOf (item);
		}
		public void Insert (int index, UITableViewCell item)
		{
			items.Insert (index, item);
			if (tableView != null)
				tableView.InsertRows (new []{ NSIndexPath.FromRowSection (index, 0) }, UITableViewRowAnimation.Automatic);
		}
		public void RemoveAt (int index)
		{
			items.RemoveAt (index);
			if (tableView != null)
				tableView.DeleteRows (new []{ NSIndexPath.FromRowSection (index, 0) }, UITableViewRowAnimation.Automatic);
		}
		public UITableViewCell this [int index] {
			get {
				return items [index];
			}
			set {
				items [index] = value;
				if (tableView != null)
					tableView.ReloadRows (new []{ NSIndexPath.FromRowSection (index, 0) }, UITableViewRowAnimation.Automatic);
			}
		}
		#endregion
		#region ICollection implementation
		public void Add (UITableViewCell item)
		{
			items.Add (item);
			if (tableView != null)
				tableView.InsertRows (new []{ NSIndexPath.FromRowSection (IndexOf(item), 0) }, UITableViewRowAnimation.Automatic);
		}
		public void InsertRange(int index, IEnumerable<UITableViewCell> cells)
		{

			if(items.Contains(cells.ElementAt(0)))
				return;

			this.items.InsertRange (index, cells);
			if (tableView != null)
				tableView.InsertRows (cells.Select(x=> NSIndexPath.FromRowSection(this.items.IndexOf(x),0)).ToArray() , UITableViewRowAnimation.Automatic);

		}
		public void RemoveRange(IEnumerable<UITableViewCell> cells)
		{
			var enumerable = cells as UITableViewCell[] ?? cells.ToArray ();
			if(!items.Contains(enumerable[0]))
				return;
			var indexPaths = enumerable.Select (x => NSIndexPath.FromRowSection (this.items.IndexOf (x), 0)).ToArray ();
			var start = this.items.IndexOf (enumerable [0]);

			this.items.RemoveRange (start, enumerable.Count ());

			if (tableView != null)
				tableView.DeleteRows (indexPaths , UITableViewRowAnimation.Automatic);
		}
		public void Clear ()
		{
			items.Clear ();
			if (tableView != null)
				tableView.ReloadData ();
		}
		public bool Contains (UITableViewCell item)
		{
			return items.Contains (item);
		}
		public void CopyTo (UITableViewCell[] array, int arrayIndex)
		{
			items.CopyTo (array, arrayIndex);
		}
		public bool Remove (UITableViewCell item)
		{
			var i =  NSIndexPath.FromRowSection (IndexOf(item), 0);

			var success =  items.Remove (item);
			if (tableView != null)
				tableView.DeleteRows (new []{i} , UITableViewRowAnimation.Automatic);
			return success;
		}
		public int Count {
			get {
				return items.Count;
			}
		}
		public bool IsReadOnly {
			get {
				return false;
			}
		}
		#endregion
		#region IEnumerable implementation
		public IEnumerator<UITableViewCell> GetEnumerator ()
		{
			return items.GetEnumerator ();
		}
		#endregion
		#region IEnumerable implementation
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return items.GetEnumerator ();
		}
		#endregion
	}
}