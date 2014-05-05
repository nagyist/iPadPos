using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class UIBorderedButton : UIButton
	{
		public UIBorderedButton()
		{
			init ();
		}
		void init()
		{
			TitleColor = TintColor;
			BorderWidth = .5f;
			CornerRadius = 5f;
			TitleLabel.AddObserver (this,new MonoTouch.Foundation.NSString("text"), MonoTouch.Foundation.NSKeyValueObservingOptions.Old | MonoTouch.Foundation.NSKeyValueObservingOptions.New,IntPtr.Zero);
		}

		public float BorderWidth
		{
			get{ return Layer.BorderWidth; }
			set{ Layer.BorderWidth = value; }
		}
		public float CornerRadius
		{
			get{ return Layer.CornerRadius; }
			set{ Layer.CornerRadius = value; }
		}
	
		public override void ObserveValue (MonoTouch.Foundation.NSString keyPath, MonoTouch.Foundation.NSObject ofObject, MonoTouch.Foundation.NSDictionary change, IntPtr context)
		{
			//base.ObserveValue (keyPath, ofObject, change, context);
			if (ofObject == TitleLabel)
				SetColor ();
		}

		void SetColor()
		{
			Layer.BorderColor = CurrentTitleColor.CGColor;
		}

		public UIColor TitleColor
		{
			get{ return TitleColor (UIControlState.Normal); }
			set{ SetTitleColor (value, UIControlState.Normal); }
		}

		public override void TintColorDidChange ()
		{
			base.TintColorDidChange ();
			TitleColor = TintColor;
		}

		public string Title
		{
			get{ return Title (UIControlState.Normal); }
			set{ SetTitle (value, UIControlState.Normal); }
		}
		public override void SizeToFit ()
		{
			base.SizeToFit ();
			var frame = Frame;
			frame.Width += 10;
			Frame = frame;
		}
	}
}

