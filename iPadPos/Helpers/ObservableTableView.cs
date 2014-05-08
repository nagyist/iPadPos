using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace iPadPos
{
	[Register ("ObservableTableView")]
	public class ObservableTableView : UITableView
	{		
		public UITableViewRowAnimation AddAnimation { get; set; }
		public UITableViewRowAnimation DeleteAnimation { get; set; }
		public Func<UITableViewCell> CreateCellFunc {get;set;}
		public Action<UITableViewCell,object> BindCellAction {get;set;}
		object dataSource;
		IList list;
		INotifyCollectionChanged notifier;

		System.Threading.Thread mainThread;

		bool loadedView = false;
		public bool AutoDeselectRows { get; set; }
		public string CellIdentifier = "C";
		public bool UnEvenRows{get;set;}
		public object DataSource {
			get {
				return dataSource;
			}
			set {
				if (dataSource == value)
					return;

				if (notifier != null) {
					notifier.CollectionChanged -= HandleCollectionChanged;					
				}
				dataSource = value;
				list = value as IList;
				notifier = value as INotifyCollectionChanged;
				if (notifier != null) {
					notifier.CollectionChanged += HandleCollectionChanged;
				}
				if (loadedView) {
					this.ReloadData ();
				}
			}
		}

		public ObservableTableView ()
			: this (UITableViewStyle.Plain)
		{
			Initialize ();
		}

		public ObservableTableView (UITableViewStyle withStyle)
			: base (RectangleF.Empty,withStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
			mainThread = System.Threading.Thread.CurrentThread;

			AddAnimation = UITableViewRowAnimation.Automatic;
			DeleteAnimation = UITableViewRowAnimation.Automatic;
			AutoDeselectRows = true;
			Source = CreateSource ();

			loadedView = true;
		}



		protected virtual ObservableTableSource CreateSource ()
		{
			return new ObservableTableSource (this);
		}

		protected virtual UITableViewCell CreateCell (string reuseId)
		{
			if (CreateCellFunc != null)
				return CreateCellFunc ();
			return new UITableViewCell (UITableViewCellStyle.Default, reuseId);
		}
		protected virtual UIView CreateHeader ()
		{
			return null;
		}

		protected virtual void BindCell (UITableViewCell cell, object item, NSIndexPath indexPath)
		{
			if (BindCellAction != null) {
				BindCellAction (cell, item);
				return;
			}
			cell.TextLabel.Text = item.ToString ();
		}

		protected virtual void OnRowSelected (object item, NSIndexPath indexPath)
		{
		}

		protected virtual UITableViewCellEditingStyle EditingStyleForRow (NSIndexPath indexPath)
		{
			return UITableViewCellEditingStyle.Delete;
		}

		void HandleCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!loadedView)
				return;

			NSAction act = () => {

				if (e.Action == NotifyCollectionChangedAction.Reset) {
					this.ReloadData ();
				}

				if (e.Action == NotifyCollectionChangedAction.Add) {
					var count = e.NewItems.Count;
					var paths = new NSIndexPath[count];
					for (var i = 0; i < count; i++) {
						paths [i] = NSIndexPath.FromRowSection (e.NewStartingIndex + i, 0);
					}
					this.InsertRows (paths, AddAnimation);
				} else if (e.Action == NotifyCollectionChangedAction.Remove) {
					var count = e.OldItems.Count;
					var paths = new NSIndexPath[count];
					for (var i = 0; i < count; i++) {
						paths [i] = NSIndexPath.FromRowSection (e.OldStartingIndex + i, 0);
					}
					this.DeleteRows (paths, DeleteAnimation);
				}
			};

			var isMainThread = System.Threading.Thread.CurrentThread == mainThread;

			if (isMainThread) {
				act ();
			} else {
				NSOperationQueue.MainQueue.AddOperation (act);
				NSOperationQueue.MainQueue.WaitUntilAllOperationsAreFinished ();
			}
		}

		protected class ObservableTableSource : UITableViewSource
		{
			readonly ObservableTableView controller;

			public ObservableTableSource (ObservableTableView controller)
			{
				this.controller = controller;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var item = controller.list != null ? controller.list [indexPath.Row] : null;
				try {
					controller.OnRowSelected (item, indexPath);
				} catch (Exception ex) {
					Debug.WriteLine (ex);
				}
				if (controller.AutoDeselectRows)
					tableView.DeselectRow (indexPath, true);
			}

			public override int NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				var coll = controller.list;
				return coll != null ? coll.Count : 0;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell (controller.CellIdentifier) ??
					controller.CreateCell (controller.CellIdentifier);

				try {
					var coll = controller.list;
					if (coll != null) {
						var obj = coll[indexPath.Row];
						controller.BindCell (cell, obj, indexPath);
					}

					return cell;
				}
				catch (Exception ex) {
					Debug.WriteLine (ex);
				}

				return cell;
			}
			public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath)
			{
				try{
					return controller.EditingStyleForRow(indexPath);
				}
				catch(Exception ex) {
					Debug.WriteLine (ex);
				}
				return UITableViewCellEditingStyle.Delete;
			}
			public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				controller.list.RemoveAt (indexPath.Row);
			}

			public override bool RespondsToSelector (MonoTouch.ObjCRuntime.Selector sel)
			{
				if (sel.Name == "tableView:viewForHeaderInSection:") {
					return controller.CreateHeader() != null;
				}
				return base.RespondsToSelector (sel);

			}

			public override UIView GetViewForHeader (UITableView tableView, int section)
			{
				return controller.CreateHeader ();
			}
		}
	}
}