using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;

namespace iPadPos
{
	public class ItemCollectionViewCell: UICollectionViewCell
	{
		public static NSString Key = new NSString ("ItemCollectionViewCell");
		UILabel label;
		[Export ("initWithFrame:")]
		public ItemCollectionViewCell (System.Drawing.RectangleF frame) : base (frame)
		{
			//BackgroundView = new UIView{BackgroundColor = UIColor.Orange};

			//SelectedBackgroundView = new UIView{BackgroundColor = UIColor.Green};

			ContentView.Layer.BorderColor = UIColor.LightGray.CGColor;
			ContentView.Layer.BorderWidth = 2.0f;
			//ContentView.Transform = CGAffineTransform.MakeScale (0.8f, 0.8f);
			label = new UILabel {
				TextColor = UIColor.White,
				ShadowColor = Color.DarkGray.ToUIColor().ColorWithAlpha(.25f),
				ShadowOffset = new System.Drawing.SizeF(1,1),
				BackgroundColor = UIColor.Clear,
				TextAlignment = UITextAlignment.Center,
			};
			Layer.BorderWidth = .5f;
			Layer.BorderColor = Color.DarkGray.ToUIColor ().CGColor;
			Layer.CornerRadius = 20;

			ContentView.AddSubview (label);
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			label.Frame = ContentView.Bounds;
		}
		Item item;
		public Item Item {
			get {
				return item;
			}
			set {
				item = value;
				label.Text = item.Description;
			}
		}
	}
}