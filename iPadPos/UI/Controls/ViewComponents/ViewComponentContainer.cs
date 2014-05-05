using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace iPadPos
{
	public class ViewComponentContainer : UIView, IViewComponent
	{
		UIView view;
		public ViewComponentContainer (UIView view)
		{
			this.view = view;
			if (view.Frame.Size == SizeF.Empty)
				SizeToFit ();
			else
				Frame = view.Frame;
			Add (view);
		}

		#region IViewComponent implementation

		public int Column {get;set;}

		public int ColumnSpan {get;set;}

		public int RowSpan {get;set;}

		public int Row {get;set;}

		#endregion

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			view.Frame = Bounds;
		}
		public override void SizeToFit ()
		{
			base.SizeToFit ();
			view.SizeToFit ();
			var frame = Frame;
			frame.Size = view.Frame.Size;
			Frame = frame;
		}
	}
}

