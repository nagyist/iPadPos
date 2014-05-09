using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class ColumnCell : UITableViewCell
	{
		public ColumnCell (string key) : base(UITableViewCellStyle.Default,key)
		{
			Add (ColumnView = new ColumnView ());
		}

		ColumnView ColumnView;

		public int Columns {
			get{ return ColumnView.Columns; }
			set{ ColumnView.Columns = value; }
		}
		public float Padding {
			get {
				return ColumnView.Padding;
			}
			set {
				ColumnView.Padding = value;
			}
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			ColumnView.Frame = ContentView.Bounds;
		}

		public override void AddSubview (UIView view)
		{
			if (view == null)
				return;
			if (view is IViewComponent)
				ColumnView.Add (view);
			else
				base.AddSubview (view);
		}
		public void AddSubview(UIView view, int column, int columnspan = 1, int row = 0, int rowspan = 1)
		{
			ColumnView.AddSubview (view, column, columnspan,row, rowspan);
		}
		public void AutoAddSubview(UIView view,int columnspan = 1)
		{
			ColumnView.AutoAddSubview (view,columnspan);
		}


	}
}

