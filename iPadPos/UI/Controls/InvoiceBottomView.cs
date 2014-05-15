using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class InvoiceBottomView : UIView
	{
		UITabBarController tabbar;
		public Action<Item> AddItem {get;set;}
		public InvoiceBottomView (UIViewController parent)
		{
			BackgroundColor = UIColor.LightGray;
			tabbar = new UITabBarController () {
				ViewControllers = new UIViewController[] {
					new QuickItemsViewController{
						ItemBackgroundColor = Color.Red,
						Title = "Coupons",
						GetItems = WebService.Main.GetCoupons,
						AddItem = (i) =>{
							if(AddItem != null)
								AddItem(i);
						},
					},
					new QuickItemsViewController{
						ItemBackgroundColor = Color.Orange,
						Title = "New Product",
						GetItems = WebService.Main.GetNewProducts,
						AddItem = (i) =>{
							if(AddItem != null)
								AddItem(i);
						},
					},
				},
			};
			AddSubview (tabbar.View);
			parent.AddChildViewController (tabbar);
		}
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			tabbar.View.Frame = Bounds;
		}
	}
}

