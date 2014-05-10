using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class DiscountViewController : UIViewController
	{
		public DiscountViewController (double price)
		{
			view.Price = price;
			PreferredContentSize = new System.Drawing.SizeF (320, 125);
		}
		public override void LoadView ()
		{
			View = new DiscountView ();
		}
		DiscountView view {get{ return View as DiscountView; }}
		public Action<double> DollarChanged {get;set;}
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			view.DollarChanged = DollarChanged;
		}
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			view.DollarChanged = null;
		}
		class DiscountView : ColumnView
		{
			public double Price {get;set;}
			public Action<double> DollarChanged {get;set;}
			NumberEntryViewConponent dollarText;
			UIBorderedButton twentyFive;
			UIBorderedButton fifty;
			UIBorderedButton dollar;
			public DiscountView()
			{
				Columns = 3;
				Init();
			}
			void Init()
			{
				AddSubview(dollarText = new NumberEntryViewConponent{
					Column = 0,
					ColumnSpan = 3,
					LabelText = "Dollar Amount",
					
				});
				dollarText.Textview.NewValue = (s) => {

				};
				AddSubview (twentyFive = new TintedButton{
					Title = "25%",
					Tapped = (b) => {
						PercentChange(.25f);
					},
				}, 0, 1, 1, 1);

				AddSubview (fifty = new TintedButton(){
					Title = "50%",
					Tapped = (b) =>{
						PercentChange(.5f);
					},
				}, 1, 1, 1, 1);

				AddSubview (dollar = new TintedButton(){
					Title = "$1",
					Tapped = (b) =>{

						DollarChanged (Price - 1);

					},
				}, 2, 1, 1, 1);
			}

			void PercentChange(float percent)
			{
				var dollarodd = Math.Round (Price * percent, 2);
				DollarChanged (dollarodd);
			}
		}
	}
}

