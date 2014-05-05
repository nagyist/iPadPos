using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class ColumnView : UIView
	{
		int columns = 11;
		public int Columns {
			get {
				return columns;
			}
			set {
				columns = value;
				this.SetNeedsLayout ();
			}
		}
		float padding = 15f;
		public float Padding {
			get { return padding; }
			set { 
				padding = value; 
				this.SetNeedsLayout ();
			}
		}

		public ColumnView ()
		{
			this.BackgroundColor = UIColor.Clear;
		}
		public void AddSubview(UIView view, int column, int columnspan = 1, int row = 0, int rowspan = 1)
		{
			var v = view as IViewComponent ?? new ViewComponentContainer (view); 
			v.Row = row;
			v.RowSpan = rowspan;
			v.Column = column;
			v.ColumnSpan = columnspan;
			AddSubview (v as UIView);
		}

		int autoColumn = 0;
		public void AutoAddSubview(UIView view,int columnspan = 1)
		{
			AddSubview (view, autoColumn, columnspan: columnspan);
			autoColumn += columnspan;
		}
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			int currentRow = 0;
			float rowHeight = 0;
			float bottom = Bounds.Height;
			float y = Padding;
			float x = Padding;
			float colWidth = ((Bounds.Width - (x * 2)) / Columns);
			foreach (var view in Subviews) {
				var element = view as IViewComponent;
				if (element == null)
					continue;
				if (currentRow != element.Row) {
					y += rowHeight + Padding;
					rowHeight = 0;
				}
				currentRow = element.Row;
				rowHeight = Math.Max (view.Frame.Height, rowHeight);
				var frame = view.Frame;
				frame.X = (element.Column * colWidth) + Padding + x;
				frame.Width = (Math.Max (element.ColumnSpan, 1) * colWidth) - (Padding * 2);
				frame.Y = y;
				if (element.RowSpan >= 2)
					frame.Height = (element.RowSpan * rowHeight) - (Padding * 2);

				view.Frame = frame;
				bottom = Math.Max (bottom, frame.Bottom);
			}

			this.Frame = new System.Drawing.RectangleF (Frame.Location, new System.Drawing.SizeF (Bounds.Width, bottom));
		}
	}
}

