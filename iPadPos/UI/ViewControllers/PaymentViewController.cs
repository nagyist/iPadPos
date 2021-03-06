﻿using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using Praeclarum.Bind;
using iOSHelpers;
using System.Threading.Tasks;

namespace iPadPos
{
	public class PaymentViewController : UIViewController
	{
		public Action InvoicePosted { get; set; }
		public PaymentViewController ()
		{
			Title = "Complete Purchase";
			NavigationItem.LeftBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, (s, e) => {
				NavigationController.PopViewControllerAnimated (true);
			});
		}

		PaymentView view{ get { return View as PaymentView; } }

		public override void LoadView ()
		{
			View = new PaymentView ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			if (Invoice == null)
				return;
			if (Invoice.Payments.Count == 0) {
				Database.Main.Table<PaymentType> ().Where (x => x.IsActive)
					.OrderBy (X => X.SortOrder).ToList ().ForEach (x => Invoice.Payments.Add (new Payment{ PaymentType = x }));
			}
			view.Payments = Invoice.Payments;
			view.PostInvoice = PostInvoice;
		}
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			view.PostInvoice = null;
		}
		public Invoice Invoice {
			get {
				return view.Invoice;
			}
			set {
				value.SalesPersonId = "";
				(View as PaymentView).Invoice = value;
			}
		}
		async Task<bool> SignIn()
		{
			var tcs = new TaskCompletionSource<bool> ();
			var alert = new UIAlertView ("Please sign in", "", null, "Cancel", "Ok");
			alert.AlertViewStyle = UIAlertViewStyle.SecureTextInput;
			var tb = alert.GetTextField(0);
			tb.ShouldReturn = (t)=>{

				alert.DismissWithClickedButtonIndex(1,true);
				signIn(tcs,tb.Text);
				return true;
			};

			alert.Clicked += async (object sender, UIButtonEventArgs e) => {
				if(e.ButtonIndex == 0)
				{
					tcs.TrySetResult(false);
					alert.Dispose();
					return;
				}

				var id = tb.Text;
				signIn(tcs,id);
			
			
			};
			alert.Show ();
			return await tcs.Task;
		}
		async void signIn(TaskCompletionSource<bool> tcs,string id)
		{
			Console.WriteLine ("Signing in");
			BigTed.BTProgressHUD.ShowContinuousProgress();
			var success = await WebService.Main.SignIn(id);
			if(!success)
				BigTed.BTProgressHUD.Dismiss ();

			if(success){
				Invoice.SalesPersonId = id;
				tcs.TrySetResult(success);
			}
			else
				tcs.TrySetResult(await SignIn());
			Console.WriteLine ("Signed in {0}", success);
		}
		async void PostInvoice()
		{
			Console.WriteLine ("Posting invoice");
			var isValid = Invoice.Validate ();
			if (!isValid.Item1) {
				App.ShowAlert ("Error", isValid.Item2);
				return;
			}

			if (string.IsNullOrEmpty(Invoice.SalesPersonId) && !await SignIn ())
				return;

			BigTed.BTProgressHUD.ShowContinuousProgress();
			var paymentSucces = await ProcessPayment ();
			if (!paymentSucces) {
				BigTed.BTProgressHUD.Dismiss ();
				return;
			}
			Console.WriteLine ("Posting invoice");
			var success = await WebService.Main.PostInvoice (Invoice);
			BigTed.BTProgressHUD.Dismiss ();
			if(!success)
			{
				new UIAlertView("Error", "There was an error posting the invoice. Please try again",null,"Ok").Show();
				return;
			}
			Settings.Shared.LastPostedChange = Invoice.Change;
			NavigationController.PopViewControllerAnimated(true);
			if (InvoicePosted != null)
				InvoicePosted ();
		}

		public async Task<bool> ProcessPayment ()
		{
			Console.WriteLine ("Processing Payment");
			if (Invoice.CardPayment == null || Invoice.CardPayment.Amount == 0)
				return true;
			if (string.IsNullOrEmpty (Invoice.Id)) {
				Invoice.Id = await WebService.Main.GetNextPostedInvoiceId ();
				Invoice.Save ();
			}
			Invoice.ChargeDetail = Database.Main.Table<ChargeDetails> ().Where (x => x.LocalInvoiceId == Invoice.LocalId).FirstOrDefault ();
			if (Invoice.ChargeDetail == null) {
				Console.WriteLine ("Awaiting Card charge");
				var charge = await CreditCardProcessor.Shared.Charge ( this,Invoice);
				if (charge == null || charge.Item1 == null){
					Invoice.ChargeDetail = null;
					new SimpleAlertView ("Credit Card Error", charge == null ? "Unknown Error":charge.Item2).Show ();
					return false;
				}
				Invoice.ChargeDetail = charge.Item1;
				Invoice.ChargeDetail.LocalInvoiceId = Invoice.LocalId;
				Database.Main.InsertOrReplace (Invoice.ChargeDetail);
			}

			Invoice.CreditCardProccessed = Invoice.ChargeDetail != null;

			if (CreditCardProcessor.Shared.NeedsSignature && Invoice.CreditCardProccessed && !Invoice.ChargeDetail.Signature.IsValid) {
				//GetSig
				BigTed.BTProgressHUD.Dismiss ();
				Invoice.ChargeDetail.Signature.Points = await GetSig ();
				BigTed.BTProgressHUD.ShowContinuousProgress ();
				Invoice.CreditCardProccessed = Invoice.ChargeDetail.Signature.IsValid;
				if (!Invoice.CreditCardProccessed)
					new SimpleAlertView ("Error", "Invalid signature").Show ();
			}
			Console.WriteLine ("Card was processed");
			return Invoice.CreditCardProccessed;
		}
		SignatureViewController sigPad;
		async Task<PointF[]> GetSig()
		{
			Console.WriteLine ("Getting sig");
			sigPad = new SignatureViewController {
				Amount = Invoice.ChargeDetail.Amount,
			};
			await PresentViewControllerAsync (new UINavigationController (sigPad), true);
			await sigPad.GetSignature ();
			await DismissViewControllerAsync (true);
			var points =  sigPad.SignatureView.Points;
			sigPad.Dispose ();
			sigPad = null;
			return points;
		}

		class PaymentView : UIView
		{
			public Action PostInvoice { get; set; }
			UIView backgroundView;
			UILabel TotalLabel;
			ObservableTableView tableView;
			UITableView rightTableView;
			UITableViewCell remaining;
			UITableViewCell change;
			UITableViewCell totalCell;
			UITableViewCell onAccountCell;
			UIButton five;
			UIButton ten;
			UIButton twenty;
			UIButton fifty;
			UIButton hundred;


			public PaymentView ()
			{
				const float rowHeight = 80;
				BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle("homeScreen"));
				Add (backgroundView = new UIView {
					BackgroundColor = UIColor.Black.ColorWithAlpha (.5f),
					ClipsToBounds = true,
					Layer = {
						BorderColor = UIColor.White.ColorWithAlpha (.75f).CGColor,
						BorderWidth = .5f,
						CornerRadius = 5,
					},
				});
				backgroundView.Add (TotalLabel = new UILabel ());
				backgroundView.Add (tableView = new ObservableTableView (UITableViewStyle.Grouped) {
					ContentInset = new UIEdgeInsets (-36, 0, 0, 0),
					BackgroundColor = UIColor.Clear,
					CellIdentifier = PaymentCell.Key,
					CreateCellFunc = () => new PaymentCell (),
					RowHeight = rowHeight,
					BindCellAction = (cell, item) => {
						(cell as PaymentCell).Payment = item as Payment;
					},
					ItemTapped = (p) =>{
						Invoice.PaymentSelected(p as Payment);
					},
				});
				var bgColor = UIColor.Black.ColorWithAlpha(.3f);
				backgroundView.Add (rightTableView = new UITableView (RectangleF.Empty, UITableViewStyle.Grouped) {
					ContentInset = new UIEdgeInsets (-36, 0, 0, 0),
					SectionHeaderHeight = 0,
					BackgroundColor = UIColor.Clear,
					ScrollEnabled = false,
					RowHeight = 60,
					Source = new CellTableViewSource {
						(onAccountCell = new SubTotalCell {
							Frame = new RectangleF (0, 0, 320, 50),
							TextLabel = {
								Text = "On Account"
							},
							BackgroundColor = bgColor
						}),
						(totalCell = new SubTotalCell {
							Frame = new RectangleF (0, 0, 320, 50),
							TextLabel = {
								Text = "Total"
							},
							BackgroundColor = bgColor
						}),
						(remaining = new TotalCell {
							Frame = new RectangleF (0, 0, 320, 65),
							TextLabel = {
								Text = "Remaining",
								TextColor = Theme.Current.PayColor,
							},
							DetailTextLabel = {
								Font = UIFont.BoldSystemFontOfSize(25),
							},
							BackgroundColor = bgColor
						}),
						(change = new TotalCell {
							Frame = new RectangleF (0, 0, 320, rowHeight),
							TextLabel = {
								Text = "Change",
								TextColor = UIColor.White,
							},
							DetailTextLabel = {
								TextColor = UIColor.White,
							},
							BackgroundColor = bgColor,
							SeparatorInset = new UIEdgeInsets (0, 0, 0, 0),
						}),
						new PayCell {
							Frame = new RectangleF (0, 0, 320, 76),
							Text = "Post",
							Tapped = async () =>{
								if(PostInvoice != null)
									PostInvoice();
							}
						}
					}
				});

				backgroundView.Add(five = new TintedButton{
					Title = "$5",
					TitleColor = UIColor.White.ColorWithAlpha(.75f),
					SelectedTintColor = Theme.Current.PayColor,
				});
				five.TouchUpInside += (object sender, EventArgs e) => {
					Invoice.CashPayment.Amount = 5;
				};
				five.SizeToFit();

				backgroundView.Add(ten = new TintedButton{
					Title = "$10",
					TitleColor = UIColor.White.ColorWithAlpha(.75f),
					SelectedTintColor = Theme.Current.PayColor,
				});
				ten.TouchUpInside += (object sender, EventArgs e) => {
					Invoice.CashPayment.Amount = 10;
				};
				ten.SizeToFit();

				backgroundView.Add(twenty = new TintedButton{
					Title = "$20",
					TitleColor = UIColor.White.ColorWithAlpha(.75f),
					SelectedTintColor = Theme.Current.PayColor,
				});
				twenty.TouchUpInside += (object sender, EventArgs e) => {
					Invoice.CashPayment.Amount = 20;
				};
				twenty.SizeToFit();

				backgroundView.Add(fifty = new TintedButton{
					Title = "$50",
					TitleColor = UIColor.White.ColorWithAlpha(.75f),
					SelectedTintColor = Theme.Current.PayColor,
				});
				fifty.TouchUpInside += (object sender, EventArgs e) => {
					Invoice.CashPayment.Amount = 50;
				};
				fifty.SizeToFit();

				backgroundView.Add(hundred = new TintedButton{
					Title = "$100",
					TitleColor = UIColor.White.ColorWithAlpha(.75f),
					SelectedTintColor = Theme.Current.PayColor,
				});
				hundred.TouchUpInside += (object sender, EventArgs e) => {
					Invoice.CashPayment.Amount = 100;
				};
				hundred.SizeToFit();


			}


			public override void LayoutSubviews ()
			{
				base.LayoutSubviews ();
				var frame = new RectangleF (0, 0, 500, 420);
				float h = 54;
				backgroundView.Frame = frame;
				var c = Center;
				c.Y -= 100;
				backgroundView.Center = c;
				frame.Height -= h;

				var rightWidth = 250;
				frame.Width -= rightWidth;

				tableView.Frame = frame;
				frame.X = frame.Right;
				frame.Width = rightWidth;
				rightTableView.Frame = frame;

				var bottom = frame.Bottom + 10;
				frame = five.Frame;

				var width = (backgroundView.Frame.Width - 60)/5;
				frame.X = 10;
				frame.Width = width;
				frame.Y = bottom;
				five.Frame = frame;

				frame.X = frame.Right + 10;
				ten.Frame = frame;

				frame.X = frame.Right + 10;
				twenty.Frame = frame;

				frame.X = frame.Right + 10;
				fifty.Frame = frame;

				frame.X = frame.Right + 10;
				hundred.Frame = frame;

			}

			public object Payments {
				get { return tableView.DataSource; }
				set{ tableView.DataSource = value; }
			}

			Invoice invoice;
			public Invoice Invoice {
				get {
					return invoice;
				}
				set {
					unBind ();
					invoice = value;
					bind ();
				}
			}
			Binding binding;
			void unBind()
			{
				if (binding == null)
					return;
				binding.Unbind ();
			}

			void bind()
			{
				binding = Binding.Create(()=>
					remaining.DetailTextLabel.Text == Invoice.RemainingString &&
					totalCell.DetailTextLabel.Text == Invoice.TotalString &&
					change.DetailTextLabel.Text == Invoice.ChangeString &&
					onAccountCell.DetailTextLabel.Text == Invoice.Customer.OnAccount.ToString("C")
				);
			}
		}
	}
}

