using System;
using MonoTouch.UIKit;

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
						ItemBackgroundColor = Color.Red,
						AlternateItemBackgroundColor = Color.Olive,
						Title = "Coupons",
						GetItems = WebService.Main.GetCoupons,
						AddItem = (i) =>{
							if(AddItem != null)
								AddItem(i);
						},
					},
					newProduct = new QuickItemsViewController{
						ItemBackgroundColor = Color.Orange,
						AlternateItemBackgroundColor = Color.Orange,
						Title = "New Product",
						GetItems = WebService.Main.GetNewProducts,
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

