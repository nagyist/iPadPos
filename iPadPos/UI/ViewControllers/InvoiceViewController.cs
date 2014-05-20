using System;
using MonoTouch.UIKit;
using iOSHelpers;

namespace iPadPos
{
	public class InvoiceViewController : BaseViewController
	{
		InvoiceView view {
			get{ return View as InvoiceView; }
		}

		SimpleActionSheet sheet;
		UIPopoverController popover;
		public InvoiceViewController ()
		{
			Title = "Best POS Ever";
			var searchBar = new ItemSearchView{ Frame = new System.Drawing.RectangleF (0, 0, 200, 30) };
			searchBar.ItemSelected += (Item obj) => {
				Invoice.AddItem (obj);
			};
			NavigationItem.RightBarButtonItem = new UIBarButtonItem (searchBar);
			NavigationItem.LeftBarButtonItem = new UIBarButtonItem (UIImage.FromBundle ("menu").ImageWithRenderingMode (UIImageRenderingMode.AlwaysTemplate), UIBarButtonItemStyle.Plain, (s, e) => {
				//Show simple actionsheet
				if (sheet != null) {
					sheet.DismissWithClickedButtonIndex (-1, true);
					return;
				}
				sheet = new SimpleActionSheet { 
					{"New Invoice",() => new SimpleAlertView ("Save invoice", "Do you want to save the current invoice?") {
							{ "Cancel",Color.Olive,NewInvoice },
							{ "Delete",Color.Red,null },
							{ "Save",Color.Olive,SaveInvoice }
						}.Show ()
					},
					{"Load Invoice",() => {
							if(popover != null)
							{
								popover.Dismiss(true);
							}
							popover = new UIPopoverController(new UINavigationController(new LoadInvoiceViewController()));
							popover.DidDismiss += (sender,  evt) => {
								popover.Dispose();
							};
							popover.PresentFromBarButtonItem(NavigationItem.LeftBarButtonItem, UIPopoverArrowDirection.Any,true);
						}
					},
				};
				sheet.Dismissed += (object sender, UIButtonEventArgs e2) => {
					sheet.Dispose ();
					sheet = null;
				};
				sheet.ShowFrom (s as UIBarButtonItem, true);
			});
		
			//this.AutomaticallyAdjustsScrollViewInsets = false;
		}

		void NewInvoice ()
		{
			Invoice.DeleteLocal ();
			Invoice = new Invoice ();
		}

		async void SaveInvoice ()
		{
			var success = await WebService.Main.SaveWorkingInvoice (Invoice);
			if (success) {
				NewInvoice ();
			}
		}

		public Invoice Invoice {
			get {
				return view.Invoice;
			}
			set {
				view.Invoice = value;
			}
		}

		public override void LoadView ()
		{
			View = new InvoiceView (this) {
				Invoice = Invoice.FromLocalId (Settings.Shared.CurrentInvoice),
			};
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			view.Parent = this;
			SocketScannerHelper.Shared.Scaned = itemScanned;
			RefreshCustomer ();
		}
		async void RefreshCustomer()
		{
			if (string.IsNullOrEmpty (Invoice.CustomerId))
				return;
			try{
				Invoice.Customer = await WebService.Main.GetCustomer(Invoice.CustomerId);
			}
			catch(Exception ex) {
				Console.WriteLine (ex);
			}
		}

		async void itemScanned (string scannedText)
		{
			var item = await WebService.Main.GetItem (scannedText);
			if (item == null)
				return;
			Invoice.AddItem (item);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			view.Parent = null;
			SocketScannerHelper.Shared.Scaned = null;
		}

		public void Checkout ()
		{
			var isReady = Invoice.IsReadyForPayment ();
			if (!isReady.Item1) {
				App.ShowAlert ("Error", isReady.Item2);
				return;
			}
			PaymentViewController paymentVc = null;
			NavigationController.PushViewController (paymentVc = new PaymentViewController {Invoice = Invoice, InvoicePosted = () => {
					Invoice.DeleteLocal ();
					Invoice = new Invoice ();
					paymentVc.Dispose ();
				}
			}, true);
		}

		class InvoiceView : UIView
		{
			//UIImageView backgroundView;
			public InvoiceViewController Parent { get; set; }

			public InvoiceTableView InvoiceTable { get; set; }

			public InvoiceSideBar SideBar { get; set; }

			public InvoiceBottomView BottomView { get; set; }

			const float sideBarWidth = 320f;
			const float bottomHeight = 240;

			public InvoiceView (UIViewController  parent)
			{
				BackgroundColor = Theme.Current.BackgroundGray;
				//Add(backgroundView = new UIImageView(UIImage.FromBundle("PaymentBG").Blur(50)));
				Add (InvoiceTable = new InvoiceTableView ());
				Add (SideBar = new InvoiceSideBar {
					Checkout = () => {
						Parent.Checkout ();
					}
				});
				Add (BottomView = new InvoiceBottomView (parent) {
					AddItem = (i) => {
						Invoice.AddItem (i);
					}
				});

				this.ConstrainLayout (() => 
//					backgroundView.Frame.X == Frame.X &&
//					backgroundView.Frame.Width == Frame.Width &&
//					backgroundView.Frame.Height == Frame.Height &&
//					backgroundView.Frame.Top == Frame.Top &&

					BottomView.Frame.Right == Frame.Right &&
				BottomView.Frame.Width == Frame.Width &&
				BottomView.Frame.Bottom == Frame.Bottom &&
				BottomView.Frame.Height == bottomHeight

				&&

				SideBar.Frame.Right == Frame.Right &&
				SideBar.Frame.Width == sideBarWidth &&
				SideBar.Frame.Bottom == BottomView.Frame.Top &&
				SideBar.Frame.Top == Frame.Top

				&&

				InvoiceTable.Frame.Left == Frame.Left &&
				InvoiceTable.Frame.Right == SideBar.Frame.Left &&
				InvoiceTable.Frame.Bottom == BottomView.Frame.Top &&
				InvoiceTable.Frame.Top == Frame.Top 
				);
			}

			Invoice invoice;

			public Invoice Invoice {
				get {
					return invoice;
				}
				set {
					invoice = value;
					InvoiceTable.Invoice = value;
					SideBar.Invoice = value;
				}
			}
		}
	}
}

