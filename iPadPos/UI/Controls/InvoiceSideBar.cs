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
		UITableViewCell lastPostedChange;
		CellTableViewSource source;
		UIPopoverController popover;

		public Action Checkout { get; set; }

		public InvoiceSideBar ()
		{
			this.Layer.BorderColor = UIColor.Black.ColorWithAlpha (.25f).CGColor;
			this.Layer.BorderWidth = .5f;
			BackgroundColor = UIColor.DarkGray;
			AddSubview (tableView = new UITableView (RectangleF.Empty, UITableViewStyle.Grouped) {
				BackgroundColor = UIColor.Clear,
				Source = source = new CellTableViewSource {
					(customer = new CustomerPickerCell {
						Tapped = async () => {
							CustomerSearchViewController customerSearch;
							popover = new UIPopoverController (new UINavigationController (customerSearch = new CustomerSearchViewController {
								CustomerPicked = (c) => {
									Invoice.Customer = c;
									popover.Dismiss (true);
									popover.Dispose ();
								}
							}) {
								NavigationBar = {
									BarStyle = UIBarStyle.BlackTranslucent,
								}
							});
							customerSearch.Popover = popover;
							popover.PresentFromRect (customer.Frame, tableView, UIPopoverArrowDirection.Right, true);
						}
					}),
					(subtotal = new SubTotalCell ()),
					(discount = new DiscountCell ()),
					(total = new TotalCell ()),
					new PayCell {
						Text = "Checkout",
						TintColor = UIColor.White,
						Tapped = () => {
							Checkout ();
						}
					}, (lastPostedChange = new LastPostedCell () {

					}),
				},
				ScrollEnabled = false,
				TableHeaderView = new UIView (new RectangleF (0, 0, 0, 64)),
			});
			Binding.Create (() => lastPostedChange.DetailTextLabel.Text == Settings.Shared.LastPostedChangeString);
			customerInfo = new UITableViewCell[] {
				(email = new MiniCell {
					TextLabel = {
						Text = "Email"
					},
					Tapped = ()=>{
						showEditor(email);
					},
				}),
				(phoneNumber = new MiniCell {
					TextLabel = {
						Text = "Phone"
					},
					Tapped = ()=>{
						showEditor(phoneNumber);
					},
				}),
				(onAccount = new MiniCell {
					TextLabel = {
						Text = "On Account"
					},
					Tapped = ()=>{
						showEditor(onAccount);
					},
				}),
			};

		}
		public void showEditor(UIView fromView)
		{
			CustomerInformationViewController customerSearch;
			popover = new UIPopoverController (new UINavigationController (customerSearch = new CustomerInformationViewController {
				Customer = Invoice.Customer,
				Created = (c) => {
					Invoice.Customer = c;
					popover.Dismiss (true);
					popover.Dispose ();
				}
			}) {
				NavigationBar = {
					BarStyle = UIBarStyle.BlackTranslucent,
				}
			});
			customerSearch.Popover = popover;
			popover.PresentFromRect (fromView.Frame, tableView, UIPopoverArrowDirection.Right, true);
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

		void bind ()
		{
			binding = Binding.Create (() => 
				total.Value == Invoice.Total &&
			subtotal.DetailTextLabel.Text == Invoice.SubtotalString
			);
			Invoice.SubscribeToProperty ("Customer", CustomerChanged);
			CustomerChanged ();
		}

		void unbind ()
		{
			if (binding == null)
				return;

			binding.Unbind ();
			Invoice.UnSubscribeToProperty ("Customer", CustomerChanged);
		}

		void CustomerChanged ()
		{
			var cust = Invoice.Customer;
			customer.TextLabel.Text = cust == null ? "Pick a Customer" : cust.ToString ();

			email.DetailTextLabel.Text = cust == null ? "" : cust.Email;
			phoneNumber.DetailTextLabel.Text = cust == null ? "" : cust.HomePhone;
			onAccount.DetailTextLabel.Text = cust == null ? "" : cust.OnAccount.ToString ("C");


			if (cust == null || string.IsNullOrEmpty (cust.CustomerId))
				source.RemoveRange (customerInfo);
			else
				source.InsertRange (1, customerInfo);

		}
	}
}

