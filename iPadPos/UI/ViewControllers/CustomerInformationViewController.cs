using System;
using MonoTouch.UIKit;
using Praeclarum.Bind;

namespace iPadPos
{
	public class CustomerInformationViewController : UIViewController
	{
		public CustomerInformationViewController ()
		{
			this.PreferredContentSize = new System.Drawing.SizeF (600, 500);
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
		}
		public Customer Customer
		{
			get {return view.Customer;}
			set { view.Customer = value; }
		}

		class CustomerInformationView : UIView
		{
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
					bind ();
				}
			}
			FormElementTextView firstName;
			public CustomerInformationView()
			{
				Add(columnView = new ColumnView());
				columnView.AddSubview(firstName = new FormElementTextView{
					LabelText = "First Name",
					Column = 0,
					ColumnSpan = 3,
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
					firstName.Textview.Text == customer.FirstName 
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
		}
	}
}

