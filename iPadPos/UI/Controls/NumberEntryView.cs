using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace iPadPos
{
	public class NumberEntryView : UITextField
	{
		UIPopoverController popover;
		public Action<string> NewValue { get; set; }
		public NumberEntryView ()
		{
			BackgroundColor = UIColor.White;
			BorderStyle = UITextBorderStyle.RoundedRect;
			this.KeyboardType = UIKeyboardType.DecimalPad;
			this.ShouldBeginEditing = (t) =>{
				if(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone || popover != null && popover.PopoverVisible)
					return true;
				var num = new NumberInputViewController(this);
				popover = new UIPopoverController(num){

				};
				popover.PresentFromRect(this.Frame,this.Superview, UIPopoverArrowDirection.Any,true);
				popover.DidDismiss += (object sender, EventArgs e) => {
					this.ResignFirstResponder();
					popover.Dispose();
					num.Dispose();
					popover = null;
				};

				this.SelectAll(this);
				return true;
			};
			this.EditingDidEnd += (s,e) => {
				if(NewValue != null)
					NewValue(Text);
				if(popover != null && popover.PopoverVisible)
					popover.Dismiss(true);
			};
			this.InputView = new UIView (new RectangleF (0, 0, 0, 0));

		}

	}
}

