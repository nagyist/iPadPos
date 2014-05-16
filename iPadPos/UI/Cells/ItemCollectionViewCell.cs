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
		UILabel bottomLabel;
		[Export ("initWithFrame:")]
		public ItemCollectionViewCell (System.Drawing.RectangleF frame) : base (frame)
		{
			ContentView.Layer.BorderColor = UIColor.LightGray.CGColor;
			ContentView.Layer.BorderWidth = 2.0f;
			//ContentView.Transform = CGAffineTransform.MakeScale (0.8f, 0.8f);
			label = new UILabel {
				TextColor = UIColor.White,
				ShadowColor = Color.DarkGray.ToUIColor().ColorWithAlpha(.25f),
				ShadowOffset = new System.Drawing.SizeF(1,1),
				BackgroundColor = UIColor.Clear,
				TextAlignment = UITextAlignment.Center,
				Lines = 2,
				LineBreakMode = UILineBreakMode.WordWrap,
			};
			ContentView.AddSubview (bottomLabel = new UILabel {
				TextColor = UIColor.White,
				ShadowColor = Color.DarkGray.ToUIColor().ColorWithAlpha(.25f),
				ShadowOffset = new System.Drawing.SizeF(1,1),
				BackgroundColor = UIColor.Clear,
				TextAlignment = UITextAlignment.Center,
				Font = UIFont.BoldSystemFontOfSize(UIFont.LabelFontSize),
			});
//			Layer.BorderWidth = .5f;
//			Layer.BorderColor = Color.DarkGray.ToUIColor ().CGColor;
			Layer.CornerRadius = 10;

			ContentView.AddSubview (label);
		}

		public override void LayoutSubviews ()
		{
			const float padding = 10;
			var frame = ContentView.Frame;
			frame.X = padding;
			frame.Width -= padding * 2;
			base.LayoutSubviews ();
			if (bottomLabel.Text == "") {
				label.Frame = frame;
				return;
			}
			var size = ContentView.Bounds.Size;
			size.Width -= 20;
			size = label.SizeThatFits (size);

			frame.Y = padding;
			frame.Height = size.Height;
			label.Frame = frame;

			frame.Y = frame.Bottom - padding;
			frame.Height = Bounds.Height - frame.Height;
			bottomLabel.Frame = frame;
		
		}
		Item item;
		public Item Item {
			get {
				return item;
			}
			set {
				item = value;
				label.Text = item.Description.UppercaseAllWords();
				bottomLabel.Text = item.Price != 0 ? item.Price.ToString ("C") : "";
				SetNeedsLayout ();
			}
		}
	}
}