using System;
using MonoTouch.UIKit;
using iOSHelpers;
using System.Drawing;

namespace iPadPos
{
	public class DiscountCell : UITableViewCell, ICellSelectable
	{
		public Action AddDiscount { get; set; }

		public DiscountCell () : base (UITableViewCellStyle.Value1, "discountcell")
		{
			TextLabel.Text = "Discount";
			SelectionStyle = UITableViewCellSelectionStyle.Gray;
			TextLabel.TextColor = UIColor.White;
			BackgroundColor = Theme.Current.SideBarCellColor;
			AccessoryView = new UIView (new RectangleF (0, 0, 40, 28)) { new SimpleButton {
					Frame = new System.Drawing.RectangleF (12, 0, 28, 28),
					Image = UIImage.FromBundle ("plus-outline").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate),
					TintColor = Color.Red,
					Tapped = (btn) => {
						if (AddDiscount != null)
							AddDiscount ();
					}
				}
			};
		}

		#region ICellSelectable implementation

		public void OnSelect ()
		{
			if (AddDiscount != null)
				AddDiscount ();
		}

		#endregion
	}
}

