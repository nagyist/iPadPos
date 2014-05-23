using System;
using MonoTouch.UIKit;
using Praeclarum.Bind;
using System.ComponentModel;
using System.Threading.Tasks;
using iOSHelpers;

namespace iPadPos
{
	public class CustomerInformationViewController : UIViewController
	{
		public UIPopoverController Popover { get; set;}
		public Action<Customer> Created { get; set;}
		public bool IsCreate { get; set;}
		public CustomerInformationViewController ()
		{
			this.PreferredContentSize = new System.Drawing.SizeF (700, 400);
			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Save, async (s, e) => {
				view.DismisKeyboard();
				BigTed.BTProgressHUD.Show();
				bool success = false;
				if(string.IsNullOrEmpty(Customer.CustomerId))
					success = await InsertCustomer();
				else
					success = await UpdateCustomer();
				BigTed.BTProgressHUD.Dismiss();
				if(!success)
				{
					new SimpleAlertView("Error", "There was an error saving the customer. Please try again.").Show();
					return;
				}
				if(Created != null)
					Created(Customer);
				if(!IsCreate)
					Popover.Dismiss(true);
			});
			this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, (s, e) => {
				view.DismisKeyboard();
				if(IsCreate)
					NavigationController.PopViewControllerAnimated(true);
				else
					Popover.Dismiss(true);
			});
		}



		async Task<bool> InsertCustomer()
		{
			Customer = await WebService.Main.CreateCustomer(Customer);
			Customer.IsNew = true;
			return !string.IsNullOrEmpty (Customer.CustomerId);
		}
		async Task<bool> UpdateCustomer ()
		{
			var success = await WebService.Main.UpdateCustomer (Customer);
			return success;
		}
		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			//Customer = null;
		}
		public override void LoadView ()
		{
			View = new CustomerInformationView();
		}
		CustomerInformationView view
		{
			get{return View as CustomerInformationView; }
		}
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			view.TopPadding = NavigationController.NavigationBar.Frame.Height;	
			if (Popover != null)
				Popover.ShouldDismiss = shouldDismiss;

		}
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			if (Popover != null)
				Popover.ShouldDismiss = null;
		}

		bool shouldDismiss(UIPopoverController pop)
		{
			view.DismisKeyboard ();
			return !Customer.IsDirty;
		}
		public Customer Customer
		{
			get {return view.Customer;}
			set { 
				if(value != null){
					value.IsDirty = false;
					IsCreate = string.IsNullOrEmpty (value.CustomerId);
					Title = string.IsNullOrEmpty (value.CustomerId) ? "Add new Customer" : "Customer ID: " + value.CustomerId;
				}				
				view.Customer = value;
			}
		}
		public bool ReadOnly
		{
			get{ return !view.Enabled; }
			set{ view.Enabled = !value; }
		}

		class CustomerInformationView : UIView, INotifyPropertyChanged
		{
			public void DismisKeyboard()
			{
				firstName.Textview.BecomeFirstResponder ();
				firstName.Textview.ResignFirstResponder ();
			}
			bool enabled = true;
			public bool Enabled {
				get {
					return enabled;
				}
				set {
					PropertyChanged.SetProperty (this, ref enabled, value);
				}
			} 
			public float TopPadding {get;set;}
			ColumnView columnView;
			Customer customer;
			public Customer Customer {
				get {
					return customer;
				}
				set {
					unbind ();
					customer = value;
					value.IsDirty = false;
					bind ();
				}
			}
			FormElementTextView firstName;
			FormElementTextView lastName;
			FormElementTextView email;
			FormElementTextView customerId;
			FormElementTextView phone;
			FormElementTextView cellPhone;
			FormElementTextView address;
			FormElementTextView address2;
			FormElementTextView city;
			FormElementTextView state;
			FormElementTextView zip;
			public CustomerInformationView()
			{
				Add(columnView = new ColumnView(){
					(firstName = new FormElementTextView{
						LabelText = "First Name",
						Row = 0,
						Column = 0,
						ColumnSpan = 5,
					}),
					(lastName = new FormElementTextView{
						LabelText = "Last Name",
						Row = 0,
						Column = 5,
						ColumnSpan = 6,
					}),
					(email = new FormElementTextView{
						LabelText = "Email",
						Row = 1,
						Column = 0,
						ColumnSpan = 5,
					}),
					(phone = new FormElementTextView{
						LabelText = "Phone",
						Row = 1,
						Column = 5,
						ColumnSpan = 3,
					}),
					(cellPhone = new FormElementTextView{
						LabelText = "Cell Phone",
						Row = 1,
						Column = 8,
						ColumnSpan = 3,
					}),
					(address = new FormElementTextView{
						LabelText = "Address",
						Row = 2,
						Column = 0,
						ColumnSpan = 11,
					}),
					(address2 = new FormElementTextView{
						LabelText = "Address 2",
						Row = 3,
						Column = 0,
						ColumnSpan = 11,
					}),
					(city = new FormElementTextView{
						LabelText = "City",
						Row = 4,
						Column = 0,
						ColumnSpan = 4,
					}),
					(state = new FormElementTextView{
						LabelText = "State",
						Row = 4,
						Column = 4,
						ColumnSpan = 2,
					}),
					(zip = new FormElementTextView{
						LabelText = "Zip",
						Row = 4,
						Column = 6,
						ColumnSpan = 3,
					}),

				});
			}
			Binding binding;
			void unbind()
			{
				if(binding == null)
					return;
				binding.Unbind ();
			}
			void bind()
			{
				binding = Binding.Create(()=>
					firstName.Textview.Text == customer.FirstName &&
					firstName.Enabled == Enabled &&

					lastName.Textview.Text == customer.LastName &&
					lastName.Enabled == Enabled &&

					email.Textview.Text == customer.Email &&
					email.Textview.Enabled == Enabled &&

					phone.Textview.Text == customer.HomePhone &&
					phone.Enabled == Enabled &&

					cellPhone.Textview.Text == customer.CellPhone &&
					cellPhone.Enabled == Enabled &&

					address.Textview.Text == customer.Address &&
					address.Enabled == Enabled &&

					address2.Textview.Text == customer.Address2 &&
					address2.Enabled == Enabled &&

					city.Textview.Text == customer.City &&
					city.Enabled == Enabled &&

					state.Textview.Text == customer.State &&
					state.Enabled == Enabled &&

					zip.Textview.Text == customer.Zip &&
					zip.Enabled == Enabled 
				);
			}
			public override void LayoutSubviews ()
			{
				base.LayoutSubviews ();
				var frame = Bounds;
				frame.Y = TopPadding;
				frame.Height -= TopPadding;
				columnView.Frame = frame;
			}

			#region INotifyPropertyChanged implementation

			public event PropertyChangedEventHandler PropertyChanged;

			#endregion
		}
	}
}

