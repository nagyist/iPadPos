using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class NumberEntryViewConponent: ViewComponent
	{
		public NumberEntryView Textview;
		public Action<string> ValueChanged { get; set; }
		public NumberEntryViewConponent (): base(new RectangleF(0,0,100,44))
		{
			inputView = Textview = new NumberEntryView (){
				AutocorrectionType = UITextAutocorrectionType.No,
				ReturnKeyType = UIReturnKeyType.Next,
				ShouldReturn = delegate {
					if(NextField != null)
						NextField.BecomeFirstResponder();
					return true;
				},
			};
			//textview.DisabledBackground = Images.DisabledBackground.Value;
			Textview.ValueChanged += (object sender, EventArgs e) => {
				if(ValueChanged != null)
					ValueChanged(Text);
			};
			Textview.TouchUpOutside += (object sender, EventArgs e) => {
				Textview.ResignFirstResponder();
			};
			this.AddSubview (label);
			this.AddSubview (Textview);
		}

		public UIResponder NextField { get; set; }

		public UIKeyboardType KeyboardType {
			get{ return Textview.KeyboardType;}
			set{ Textview.KeyboardType = value;}
		}
		public string Text { 
			get{ return Textview.Text;}
			set{ Textview.Text = value;}
		}

		public bool Enabled {
			get{ return Textview.Enabled;}
			set{ Textview.Enabled = value;
				//textview.BackgroundColor = ;//value ? UIColor.White : Theme.TextBoxDisabledColor;
			}
		}

		public override bool BecomeFirstResponder ()
		{
			return Textview.BecomeFirstResponder ();
		}
	}
}