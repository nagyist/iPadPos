using System;
using MonoTouch.UIKit;
using iOSHelpers;
using System.Threading.Tasks;
using System.Linq;

namespace iPadPos
{
	public class InvoiceViewController : BaseViewController
	{
		InvoiceView view {
			get{ return View as InvoiceView; }
		}

		SimpleActionSheet sheet;
		UIPopoverController popover;
		const string title = "Best POS Ever";
		public InvoiceViewController ()
		{
			SetTitle ();
			Settings.Shared.SubscribeToProperty ("TestMode", SetTitle);
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
					{"New Invoice",async() =>{
							if(await AskSave())
								NewInvoice();
						}
					},
					{"Load Invoice",() => {
							if(popover != null)
							{
								popover.Dismiss(true);
							}
							popover = new UIPopoverController(new UINavigationController(new LoadInvoiceViewController(){
								InvoiceSelected = async (i) =>{
									popover.Dismiss(true);
									try{
										BigTed.BTProgressHUD.ShowContinuousProgress();
										if(Invoice != null && Invoice.Id != i.Id)
										{
											if(!await AskSave())
												return;
										}
										Invoice.DeleteLocal();
										Invoice = await WebService.Main.GetInvoice(i.Id);
										Invoice.Save(true);
									}
									catch(Exception ex)
									{
										Console.WriteLine(ex);
									}
									finally{
										BigTed.BTProgressHUD.Dismiss();
									}
								},
							}));
							popover.DidDismiss += (sender,  evt) => {
								popover.Dispose();
							};
							popover.PresentFromBarButtonItem(NavigationItem.LeftBarButtonItem, UIPopoverArrowDirection.Any,true);
						}
					},
					{"Payout Buy",() => {
							if(popover != null)
							{
								popover.Dismiss(true);
							}
							popover = new UIPopoverController(new UINavigationController(new LoadBuyPayoutViewController(){
								InvoiceSelected = async (i) =>{
									popover.Dismiss(true);
									try{
										BigTed.BTProgressHUD.ShowContinuousProgress();
										if(Invoice != null && Invoice.Id != i.Id)
										{
											if(!await AskSave())
												return;
										}
										Invoice.DeleteLocal();
										Invoice = await WebService.Main.GetInvoice(i.Id);
										//Setup payments
										Database.Main.Table<PaymentType> ().Where (x => x.IsActive)
											.OrderBy (X => X.SortOrder).ToList ().ForEach (x => Invoice.Payments.Add (new Payment{ PaymentType = x }));
										Invoice.Save(true);
										if((i as BuyInvoice).IsOnAccount)
											Invoice.OnAccountPayment.Amount = Invoice.Total;
										else
											Invoice.CashPayment.Amount = Invoice.Total;
									}
									catch(Exception ex)
									{
										Console.WriteLine(ex);
									}
									finally{
										BigTed.BTProgressHUD.Dismiss();
									}
								},
							}));
							popover.DidDismiss += (sender,  evt) => {
								popover.Dispose();
							};
							popover.PresentFromBarButtonItem(NavigationItem.LeftBarButtonItem, UIPopoverArrowDirection.Any,true);

						}
					},
					{"Settings",() => this.PresentViewControllerAsync (new UINavigationController (new SettingsViewController ()), true)},
				};
				sheet.Dismissed += (object sender, UIButtonEventArgs e2) => {
					sheet.Dispose ();
					sheet = null;
				};
				sheet.ShowFrom (s as UIBarButtonItem, true);
			});
		
			//this.AutomaticallyAdjustsScrollViewInsets = false;
		}
		public void SetTitle()
		{
			Title = Settings.Shared.TestMode ? string.Format ("{0} - TEST MODE", title) : title;
		}
		async Task<bool> AskSave()
		{
			if (Invoice.Items.Count == 0)
				return true;
			var tcs = new TaskCompletionSource<bool> ();
			new SimpleAlertView ("Save invoice", "Do you want to save the current invoice?") {
				{ "Save",Color.Olive,async () =>{
						var success = await SaveInvoice();
						tcs.TrySetResult(success);
					}},
				{ "Delete",Color.Red,async () =>{
						var success = await DeleteInvoice();
						tcs.TrySetResult(success);
					}},
				{ "Cancel",Color.Olive,() => tcs.TrySetResult(false) },
			}.Show ();
			return await tcs.Task;
		}

		void NewInvoice ()
		{
			Invoice.DeleteLocal ();
			Invoice = new Invoice ();
		}

		async Task<bool> SaveInvoice ()
		{
			try{
				BigTed.BTProgressHUD.ShowContinuousProgress();
				var success = await WebService.Main.SaveWorkingInvoice (Invoice);
				return success;
			}
			catch(Exception ex) {
				new SimpleAlertView ("Error saving invoice", "").Show ();
			}
			finally{
				BigTed.BTProgressHUD.Dismiss ();
			}
			return false;
		}
		async Task<bool> DeleteInvoice()
		{
			try{
				BigTed.BTProgressHUD.ShowContinuousProgress();
				bool success = false;
				if(Invoice.RecordId > 0)
					success = await WebService.Main.DeleteWorkingInvoice (Invoice);
				Invoice.DeleteLocal();
				success = true;
				return success;
			}
			catch(Exception ex) {
				new SimpleAlertView ("Error deleting invoice", "").Show ();
			}
			finally{
				BigTed.BTProgressHUD.Dismiss ();
			}
			return false;
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

			async Task<double> AskForValue(string reason,string extra)
			{
				var inputAlert = new SimpleAlertView (reason, extra) {
					{"Cancel",null},
					{"Ok",null},
				};
				inputAlert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
				inputAlert.Show ();
				var index = await inputAlert.ClickedAsync ();
				if (index == 0)
					return 0;
				double amount;
				if (double.TryParse (inputAlert.TextEntryValue, out amount))
					return amount;
				return await AskForValue (reason, "Invalid amount");
			}

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
					AddItem = async (i) => {
						var c = i as Coupon;
						if(c != null && c.ManualDiscount)
						{
							var amount = await AskForValue("Coupon Amount","");
							if(amount == 0)
								return;
							i.Price = amount;
						}
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

