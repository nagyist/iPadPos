using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class CustomerCell : UITableViewCell
	{
		Lazy<UIImage> image  = new Lazy<UIImage>(()=> UIImage.FromBundle("contacts"));
		public CustomerCell ()
		{
			TextLabel.Text = "Customer";
			TextLabel.TextColor = UIColor.White;
			BackgroundColor = Theme.Current.SideBackgroundColor.ColorWithAlpha(.5f);
			ImageView.TintColor = UIColor.White;
			ImageView.Image = image.Value.ImageWithRenderingMode (UIImageRenderingMode.AlwaysTemplate);
		}
	}
}

