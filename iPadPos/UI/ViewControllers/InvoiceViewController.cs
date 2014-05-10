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

		public InvoiceViewController ()
		{
			Title = "Best POS Ever";
			var searchBar = new ItemSearchView{ Frame = new System.Drawing.RectangleF (0, 0, 200, 30) };
			searchBar.ItemSelected += (Item obj) => {
				Invoice.AddItem(obj);
			};
			NavigationItem.RightBarButtonItem = new UIBarButtonItem (searchBar);
			//this.AutomaticallyAdjustsScrollViewInsets = false;
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
			View = new InvoiceView {
				Invoice = new Invoice()
			};
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			view.Parent = this;
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			view.Parent = null;
		}

		class InvoiceView : UIView
		{
			//UIImageView backgroundView;
			public InvoiceViewController Parent { get; set; }

			public InvoiceTableView InvoiceTable { get; set; }

			public InvoiceSideBar SideBar { get; set; }

			public InvoiceBottomView BottomView {get;set;}

			const float sideBarWidth = 320f;
			const float bottomHeight = 100;

			public InvoiceView ()
			{
				BackgroundColor = Theme.Current.BackgroundGray;
				//Add(backgroundView = new UIImageView(UIImage.FromBundle("PaymentBG").Blur(50)));
				Add (InvoiceTable = new InvoiceTableView ());
				Add (SideBar = new InvoiceSideBar{
					Checkout = ()=>{
						PaymentViewController paymentVc = null;
						Parent.NavigationController.PushViewController(paymentVc = new PaymentViewController{Invoice = Invoice, InvoicePosted = () => {
								Invoice = new Invoice();
								paymentVc.Dispose();
							}
						},true);
					}
				});
				Add (BottomView = new InvoiceBottomView());

				this.ConstrainLayout(()=> 
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

