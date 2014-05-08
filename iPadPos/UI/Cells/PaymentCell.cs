using System;
using MonoTouch.UIKit;
using Praeclarum.Bind;
using System.Drawing;

namespace iPadPos
{
	public class PaymentCell : UITableViewCell
	{
		UITextField input;
		public const string Key = "PaymentCell";
		public PaymentCell () : base(UITableViewCellStyle.Value1, Key)
		{
			BackgroundColor = UIColor.Clear;//White.ColorWithAlpha(.10f);
			DetailTextLabel.Text = "$9,999,000.00";
			TextLabel.TextColor = UIColor.White;
			DetailTextLabel.TextColor = UIColor.Clear;
			DetailTextLabel.Font = UIFont.SystemFontOfSize (20);
			input = new NumberEntryView{
				TextAlignment = UITextAlignment.Right,
				KeyboardType = UIKeyboardType.DecimalPad,
			};
			AddSubview (input);
		}
		Payment payment;
		public Payment Payment {
			get {
				return payment;
			}
			set {
				unbind ();
				payment = value;
				bind ();
			}
		}
		Binding binding;
		void unbind()
		{
			if (binding == null)
				return;
			binding.Unbind ();
		}
		void bind()
		{
			binding = Binding.Create(()=>
				TextLabel.Text == Payment.PaymentType.Description &&
				input.Text == payment.AmountString
			);
		}
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			input.Frame =  DetailTextLabel.Frame;
		}
	}
}

