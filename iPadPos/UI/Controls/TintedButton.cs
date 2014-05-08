using System;
using iOSHelpers;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class TintedButton : UIBorderedButton
	{
		public TintedButton ()
		{
		}
		public UIColor SelectedTintColor {get;set;}
		UIColor orgColor;
		public override void TouchesBegan (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			orgColor = this.TintColor;
			TintColor = SelectedTintColor;
			base.TouchesBegan (touches, evt);
		}
		public override void TouchesCancelled (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			TintColor = orgColor;
			base.TouchesCancelled (touches, evt);
		}
		public override void TouchesEnded (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			TintColor = orgColor;
			base.TouchesEnded (touches, evt);
		}
		public UIImage Image
		{
			get { return BackgroundImageForState(UIControlState.Normal); }
			set
			{
				SetBackgroundImage(value.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate), UIControlState.Normal);
				//SetBackgroundImage(value.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate), UIControlState.Highlighted);
			}
		}

	}
}

