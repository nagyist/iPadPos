using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class CustomerPickerCell : UITableViewCell, ICellSelectable
	{
		Lazy<UIImage> image  = new Lazy<UIImage>(()=> UIImage.FromBundle("contacts"));
		public Action Tapped = ()=>{};
		public CustomerPickerCell () : base(UITableViewCellStyle.Default,"contactsCell")
		{
			this.Frame = new System.Drawing.RectangleF (0, 0, 320, 60);
			TextLabel.Text = "Pick a Customer";
			TextLabel.TextAlignment = UITextAlignment.Center;
			TextLabel.TextColor = UIColor.White;
			TextLabel.Font = UIFont.BoldSystemFontOfSize (TextLabel.Font.PointSize);
			BackgroundColor = Theme.Current.SideBackgroundColor.ColorWithAlpha(.5f);
			ImageView.TintColor = UIColor.White;
			ImageView.Image = image.Value.ImageWithRenderingMode (UIImageRenderingMode.AlwaysTemplate);
			SeparatorInset = new UIEdgeInsets (0, 0, 0, 0);
		}

		#region ICellSelectable implementation

		public void OnSelect ()
		{
			Tapped ();
		}

		#endregion
	}
}

