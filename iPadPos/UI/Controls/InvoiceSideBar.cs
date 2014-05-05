using System;
using MonoTouch.Dialog;
using Praeclarum.Bind;
using MonoTouch.UIKit;
using System.Drawing;

namespace iPadPos
{
	public class InvoiceSideBar : UIView
	{
		CustomerCell customer;
		SubTotalCell subtotal;
		DiscountCell discount;
		DiscountCell tax;
		TotalCell total;
		UITableView tableView;
		public InvoiceSideBar ()
		{
			AddSubview (tableView = new UITableView(RectangleF.Empty, UITableViewStyle.Grouped){
				Source = new CellTableViewSource(new UITableViewCell[]{
					(customer = new CustomerCell()),
					(subtotal = new SubTotalCell()),
					(discount = new DiscountCell()),
					(total = new TotalCell()),
					new PayCell{
						Text = "Checkout",
						TintColor = UIColor.White,
					}
				}),
				ScrollEnabled = false,
				TableHeaderView = new UIView(new RectangleF(0,0,0,64)),
			});

		}
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			tableView.Frame = Bounds;
		}

		Invoice invoice;
		public Invoice Invoice {
			get {
				return invoice;
			}
			set {
				unbind ();
				invoice = value;
				bind ();
			}
		}

		Binding binding;
		void bind()
		{
			binding  = Binding.Create(() => 
				total.DetailTextLabel.Text == Invoice.TotalString &&
				subtotal.DetailTextLabel.Text == Invoice.SubtotalString
			);
		}
		void unbind()
		{
			if (binding != null)
				binding.Unbind ();
		}
	}
}

