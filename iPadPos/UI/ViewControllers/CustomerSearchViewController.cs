using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Linq;

namespace iPadPos
{
	public class CustomerSearchViewController : UIViewController
	{
		UISearchBar searchBar;
		UITableView tableView;
		SearchSource source;
		public Action<Customer> CustomerPicked { get; set; }
		public UIPopoverController Popover { get; set;}
		public CustomerSearchViewController ()
		{
			this.Title = "Customer Search";
			this.EdgesForExtendedLayout = UIRectEdge.None;
			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add,(s,e)=>{
				this.NavigationController.PushViewController(new CustomerInformationViewController{
					Customer = new Customer(),
					Popover = Popover,
					Created = (c) =>{
						CustomerPicked (c);
					}
				},true);
			});
			PreferredContentSize = new System.Drawing.SizeF (700, 400);
			View.Add (searchBar = new UISearchBar{
				//SearchBarStyle = UISearchBarStyle.Minimal,
				BarStyle = UIBarStyle.Black,
				Translucent = true,
			});
			searchBar.SearchButtonClicked += async (object sender, EventArgs e) => {
				source.State = SearchSource.SearchState.Searching;
				tableView.ReloadData();

				source.Customers = await WebService.Main.SearchCustomer(searchBar.Text);
				source.State = SearchSource.SearchState.Completed;
				tableView.ReloadData();

			};
			var tv = getTextField (searchBar);
			searchBar.Subviews.ForEach (x => {
				Console.WriteLine(x.GetType());
			});
			searchBar.Subviews.OfType<UITextField> ().ForEach (x => {
				x.TextColor = UIColor.White;
			});
			searchBar.SizeToFit ();
			View.Add (tableView = new UITableView{
				Source = (source = new SearchSource()),
				RowHeight = 75,
			});
		}

		UITextField getTextField(UIView view)
		{
			foreach (var v in view.Subviews) {
				if (v is UITextField)
					return v as UITextField;
				else
					return getTextField (v);
			}
			return null;
		}


		public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();
			var frame = View.Bounds;

			var vFrame = searchBar.Frame;
			vFrame.Width = frame.Width;
			searchBar.Frame = vFrame;

			vFrame.Y = vFrame.Height;
			vFrame.Height = frame.Height - vFrame.Y;
			tableView.Frame = vFrame;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			searchBar.BecomeFirstResponder ();
			source.Parent = this;
		
		}
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			source.Parent = null;
		}

		class SearchSource : UITableViewSource
		{
			public CustomerSearchViewController Parent {get;set;}
			public enum SearchState
			{
				New,
				Completed,
				Searching,

			}
			public SearchState State {get;set;}
			public List<Customer> Customers = new List<Customer>();
			public override int RowsInSection (UITableView tableview, int section)
			{
				return Customers.Count > 0 ? Customers.Count : (State == SearchState.New ? 0 : 1);
			}
			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				if (Customers.Count == 0) {
					if (State == SearchState.Searching) {
						var spinner = new MonoTouch.Dialog.ActivityElement ();
						return spinner.GetCell (tableView);
					}

					var c = (tableView.DequeueReusableCell ("default") ?? new UITableViewCell (UITableViewCellStyle.Value2, "default"));
					c.TextLabel.Text = "No Results";
					return c;
				}

				var cell = tableView.DequeueReusableCell<CustomerCell> (CustomerCell.Key);
				cell.Customer = Customers [indexPath.Row];
				return cell;
			}
			public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				if (Customers.Count == 0)
					return;
				var cust = Customers [indexPath.Row];
				Parent.CustomerPicked (cust);
			}
		}
	}
}

