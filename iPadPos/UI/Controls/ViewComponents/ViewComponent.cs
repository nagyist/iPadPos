using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace iPadPos
{
	public class ViewComponent : UIView, IViewComponent
	{
		protected UIView inputView {get;set;}

		protected UILabel label;		

		public string LabelText { 
			get { return label.Text; } 
			set { label.Text = value; } }

		public ViewComponent(RectangleF rect) : base(rect)
		{
			this.BackgroundColor = UIColor.Clear;	
			label = new UILabel (){
				Font = Theme.Current.FormLabelFont.Value,
				TextColor = Theme.Current.FormLabelColor,
				BackgroundColor = UIColor.Clear,
			};
			this.AddSubview (label);
		}
		public int Column { get; set;}
		public int ColumnSpan {get;set;}
		public int Row {get;set;}
		public int RowSpan{ get; set; }

		public static int standardPadding = 20;
		public override void LayoutSubviews ()
		{
			var h = Bounds.Height / 3;
			var frame = Bounds;
			frame.Y -= 5;
			frame.Height = h;
			label.Frame = frame;

			frame.Y = h;
			frame.Height = h * 2;
			inputView.Frame = frame;
		}
	}
}
