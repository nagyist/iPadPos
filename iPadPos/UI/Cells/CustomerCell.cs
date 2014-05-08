using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class CustomerCell : UITableViewCell
	{
		public const string Key = "CustomerCell";
		public CustomerCell () : base (UITableViewCellStyle.Value1, Key)
		{
			DetailTextLabel.TextColor = UIColor.Blue;
			ContentView.AddSubview (phone = new UILabel {
				Font = UIFont.SystemFontOfSize(15),
				TextColor = UIColor.DarkGray,
				Text = "907-947-9195"
			});
			phone.SizeToFit ();
			ContentView.AddSubview (cellPhone = new UILabel {
				Font = UIFont.SystemFontOfSize(15),
				TextColor = UIColor.DarkGray,
				//TextColor = TintColor,
			});
		}
		UILabel phone;
		UILabel cellPhone;

		Customer customer;
		public Customer Customer {
			get {
				return customer;
			}
			set {
				customer = value;
				SetValues ();
			}
		}

		void SetValues()
		{
			TextLabel.Text = Customer.ToString ();
			DetailTextLabel.Text = Customer.Email;
			phone.Text = customer.HomePhone;
			cellPhone.Text = customer.CellPhone;
		}
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var frame = phone.Frame;
			frame.X = TextLabel.Frame.Left;
			frame.Y = TextLabel.Frame.Bottom + 5;
			phone.Frame = frame;

			frame.X = frame.Right + 20;
			cellPhone.Frame = frame;

		}
	}
}

