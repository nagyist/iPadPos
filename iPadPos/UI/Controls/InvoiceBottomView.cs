using System;
using MonoTouch.UIKit;
using System.Linq;

namespace iPadPos
{
	public class InvoiceBottomView : UIView
	{
		UITabBarController tabbar;
		public Action<Item> AddItem {get;set;}
		QuickItemsViewController coupons;
		QuickItemsViewController newProduct;
		public InvoiceBottomView (UIViewController parent)
		{
			BackgroundColor = UIColor.LightGray;
			tabbar = new UITabBarController () {
				ViewControllers = new UIViewController[] {
					coupons = new QuickItemsViewController{
						TabBarItem = new UITabBarItem ("Offers", UIImage.FromBundle ("coupon"), 0),
						ItemBackgroundColor = Color.Red,
						AlternateItemBackgroundColor = Color.Olive,
						Title = "Offers",
						Sections = 2,
						GetItems = WebService.Main.GetGroupedCoupons,
						AddItem = (i) =>{
							if(AddItem != null)
								AddItem(i);
						},
					},
					newProduct = new QuickItemsViewController{
						TabBarItem = new UITabBarItem ("New Product", UIImage.FromBundle ("newProduct"), 1),
						ItemBackgroundColor = Color.Orange,
						AlternateItemBackgroundColor = Color.Orange,
						Title = "New Product",
						GetItems = (s) => WebService.Main.GetNewProducts(),
						AddItem = (i) =>{
							if(AddItem != null)
								AddItem(i);
						},
					},
				},
			};
			NotificationCenter.Shared.CouponsChanged += () => coupons.ReloadData ();
			NotificationCenter.Shared.NewProductChanged += () => newProduct.ReloadData ();
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

