using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class CustomerCell : UITableViewCell
	{
		public const string Key = "CustomerCell";
		public CustomerCell () : base (UITableViewCellStyle.Value2, Key)
		{

		}

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
		}
	}
}

