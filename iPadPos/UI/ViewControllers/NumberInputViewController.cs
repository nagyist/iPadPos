using System;
using MonoTouch.UIKit;
using iOSHelpers;
using System.Linq;
using System.Drawing;

namespace iPadPos
{
	public class NumberInputViewController : UIViewController
	{
		UITextField field;

		public NumberInputViewController (UITextField field)
		{
			this.field = field;
			this.PreferredContentSize = new SizeF (320, 400);

		}
		public override void LoadView ()
		{
			View = new NumberInputView {ButtonPressed = buttonPressed, EndEditing = EndEditing};
		}


		public void buttonPressed(string val)
		{
			var text = field.Text;
			if(val == "del"){
				field.Text = text.Length > 0 ? text.Substring(0,text.Length - 1) : "";
				return;
			}
			field.InsertText(val);
		}

		public void EndEditing()
		{
			field.ResignFirstResponder ();
		}

		class NumberInputView : UIView
		{
			public Action<string> ButtonPressed{get;set;}
			public new Action EndEditing{get;set;}
			StackPanel numberPanel;
			TintedButton done;
			public NumberInputView()
			{
				BackgroundColor = UIColor.Black.ColorWithAlpha(.5f);
				Add(numberPanel = new StackPanel{
					Columns = 3,
					Padding = 5,
				});

				Enumerable.Range(1,9).ForEach(x=> numberPanel.Add(createButton(x.ToString())));
				numberPanel.Add(createButton("."));
				numberPanel.Add(createButton("0"));
				numberPanel.Add(createButton("del"));

				done = new TintedButton {
					Layer = {
						CornerRadius = 5
					},
					BackgroundColor = UIColor.White,
					Frame = new RectangleF(0,0,44,44),
					Title = "Done",
					Font = UIFont.SystemFontOfSize(30),
					SelectedTintColor = UIColor.Black,
					TintColor = TintColor,
				};
				done.TouchUpInside += (object sender, EventArgs e) =>{
					EndEditing();
				};
				Add(done);
			}

			UIButton createButton(string text)
			{
				const float width = 80;
				var btn = new TintedButton {
					Layer = {
						CornerRadius = 5
					},
					BackgroundColor = UIColor.White,
					Frame = new RectangleF(0,0,width,width),
					Title = text,
					Font = UIFont.SystemFontOfSize(30),
					SelectedTintColor = TintColor,
					TintColor = UIColor.Black,
				};
				btn.TouchUpInside += (object sender, EventArgs e) => ButtonPressed (text);
				return btn;
			}

			public override void LayoutSubviews ()
			{
				base.LayoutSubviews ();
				var frame = Bounds;
				frame.Height = 350;
				numberPanel.Frame = frame;

				frame.Y = frame.Bottom;
				frame.X = 5;
				frame.Width -= 10;
				frame.Height = 44;
				done.Frame = frame;
			}
		}
	}
}

