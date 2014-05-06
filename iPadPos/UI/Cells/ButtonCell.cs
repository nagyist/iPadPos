using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class ButtonCell : UITableViewCell, ICellSelectable
	{
		public Action Clicked = ()=>{};
		public ButtonCell () : this("button")
		{
			//TextLabel.Layer.BorderWidth = .5f;
			//TextLabel.Layer.CornerRadius = 5;

		}

		public ButtonCell(string key) : base(UITableViewCellStyle.Default,key)
		{

			TextLabel.Layer.BorderColor = TintColor.CGColor;
			TextLabel.TextColor = TintColor;

			TextLabel.TextAlignment = UITextAlignment.Center;
		}

		public override void TintColorDidChange ()
		{
			base.TintColorDidChange ();

			TextLabel.Layer.BorderColor = TintColor.CGColor;
			TextLabel.TextColor = TintColor;
		}
		public string Text
		{
			get{ return TextLabel.Text; }
			set { TextLabel.Text = value; }
		}

//		public override void LayoutSubviews ()
//		{
//			base.LayoutSubviews ();
//			const float padding = 5;
//
//			var frame = TextLabel.Frame;
//			frame.Y = padding;
//			frame.Height -= padding * 2;
//			TextLabel.Frame = frame;
//		}

		#region ICellSelectable implementation

		public virtual void Tapped ()
		{
			Clicked ();
		}

		#endregion
	}
}

