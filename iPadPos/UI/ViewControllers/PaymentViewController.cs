using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using Praeclarum.Bind;

namespace iPadPos
{
	public class PaymentViewController : UIViewController
	{
		public PaymentViewController ()
		{
			Title = "Complete Purchase";
			NavigationItem.LeftBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, (s, e) => {
				NavigationController.PopViewControllerAnimated (true);
			});
		}

		PaymentView view;

		public override void LoadView ()
		{
			View = view = new PaymentView ();
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
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public Invoice Invoice {
			get {
				return view.Invoice;
			}
			set {
				(View as PaymentView).Invoice = value;
			}
		}



		class PaymentView : UIView
		{
			UIView backgroundView;
			UILabel TotalLabel;
			ObservableTableView tableView;
			UITableView rightTableView;
			UITableViewCell remaining;
			UITableViewCell change;
			UITableViewCell totalCell;
			UITableViewCell onAccountCell;

			public PaymentView ()
			{
				BackgroundColor = UIColor.DarkGray;
				Add (backgroundView = new UIView {
					BackgroundColor = UIColor.White.ColorWithAlpha (.25f),
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
					BackgroundColor = UIColor.Black.ColorWithAlpha (.25f),
					CellIdentifier = PaymentCell.Key,
					CreateCellFunc = () => new PaymentCell (),
					BindCellAction = (cell, item) => {
						(cell as PaymentCell).Payment = item as Payment;
					},
				});
				backgroundView.Add (rightTableView = new UITableView (RectangleF.Empty, UITableViewStyle.Grouped) {
					ContentInset = new UIEdgeInsets (-36, 0, 0, 0),
					SectionHeaderHeight = 0,
					ScrollEnabled = false,
					Source = new CellTableViewSource {
						(onAccountCell = new SubTotalCell {
							TextLabel = {
								Text = "On Account"
							}
						}),
						(totalCell = new SubTotalCell {
							TextLabel = {
								Text = "Total"
							}
						}),
						(remaining = new TotalCell {
							TextLabel = {
								Text = "Remaining",
								TextColor = UIColor.White,
							}
						}),
						(change = new TotalCell {
							TextLabel = {
								Text = "Change",
								TextColor = UIColor.White,
							},
							SeparatorInset = new UIEdgeInsets (0, 0, 0, 0),
						}),
						new PayCell {
							Frame = new RectangleF (0, 0, 320, 100),
							Text = "Post"
						}
					}
				});


			}

			public override void LayoutSubviews ()
			{
				base.LayoutSubviews ();
				var frame = new RectangleF (0, 0, 500, 274);
				backgroundView.Frame = frame;
				var c = Center;
				c.Y -= 100;
				backgroundView.Center = c;

				var rightWidth = 250;
				frame.Width -= rightWidth;

				tableView.Frame = frame;
				frame.X = frame.Right;
				frame.Width = rightWidth;
				rightTableView.Frame = frame;
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

