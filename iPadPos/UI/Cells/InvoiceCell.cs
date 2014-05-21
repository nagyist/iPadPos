using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class InvoiceCell : UITableViewCell
	{
		public const string Key = "InvoiceCell";
		public InvoiceCell () : base(UITableViewCellStyle.Value1,Key)
		{


		}
		Invoice invoice;
		public Invoice Invoice {
			get {
				return invoice;
			}
			set {
				invoice = value;
				bind ();
			}
		}
		void bind()
		{
			TextLabel.Text = invoice.CustomerName;
			DetailTextLabel.Text = invoice.TotalString;
		}
	}
}

