using System;
using MonoTouch.Dialog;
using Praeclarum.Bind;
using MonoTouch.UIKit;
using System.Drawing;

namespace iPadPos
{
	public class InvoiceSideBar : UIView
	{
		CustomerPickerCell customer;
		SubTotalCell subtotal;
		DiscountCell discount;
		DiscountCell tax;
		TotalCell total;
		UITableView tableView;

		UITableViewCell[] customerInfo = new UITableViewCell[0];
		UITableViewCell email;
		UITableViewCell phoneNumber;
		UITableViewCell onAccount;
		CellTableViewSource source;
		UIPopoverController popover;
		public Action Checkout { get; set; }
		public InvoiceSideBar ()
		{
			AddSubview (tableView = new UITableView(RectangleF.Empty, UITableViewStyle.Grouped){
				Source = source = new CellTableViewSource{
					(customer = new CustomerPickerCell{
						Tapped = async ()=>{
							popover = new UIPopoverController(new CustomerSearchViewController{
								CustomerPicked = (c)=>{
									Invoice.Customer = c;
									popover.Dismiss(true);
									popover.Dispose();
								}
							});
							popover.PresentFromRect(customer.Frame,tableView,UIPopoverArrowDirection.Right,true);
						}
					}),
					(subtotal = new SubTotalCell()),
					(discount = new DiscountCell()),
					(total = new TotalCell()),
					new PayCell{
						Text = "Checkout",
						TintColor = UIColor.White,
						Tapped = ()=>{
							Checkout();
						}
					}
				},
				ScrollEnabled = false,
				TableHeaderView = new UIView(new RectangleF(0,0,0,64)),
			});

			customerInfo = new UITableViewCell[] {
				(email = new MiniCell {
					TextLabel = {
						Text = "Email"
					}
				}),
				(phoneNumber = new MiniCell {
					TextLabel = {
						Text = "Phone"
					}
				}),
				(onAccount = new MiniCell {
					TextLabel = {
						Text = "On Account"
					}
				}),
			};

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
				total.Value == Invoice.Total &&
				subtotal.DetailTextLabel.Text == Invoice.SubtotalString
			);
			Invoice.SubscribeToProperty ("Customer", CustomerChanged);
		}
		void unbind()
		{
			if (binding == null)
				return;

			binding.Unbind ();
			Invoice.UnSubscribeToProperty ("Customer", CustomerChanged);
		}
		void CustomerChanged()
		{
			var cust = Invoice.Customer;
			customer.TextLabel.Text = cust == null ? "Pick a Customer" : cust.ToString() ;
			if (cust == null || string.IsNullOrEmpty(cust.CustomerId)) {
				source.RemoveRange (customerInfo);
				return;
			}

			email.DetailTextLabel.Text = cust == null ? "" : cust.Email;
			phoneNumber.DetailTextLabel.Text = cust == null ? "" : cust.Phone;
			onAccount.DetailTextLabel.Text = cust == null ? "" : cust.OnAccount.ToString("C");

			source.InsertRange (1, customerInfo);


		}
	}
}

