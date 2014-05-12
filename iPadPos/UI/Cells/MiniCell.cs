using System;
using MonoTouch.UIKit;

namespace iPadPos
{
	public class MiniCell : UITableViewCell, ICellSelectable
	{
		public Action Tapped { get; set; }
		public const string Key = "MiniCell";
		public MiniCell () : base (UITableViewCellStyle.Value1,Key)
		{
			Frame = new System.Drawing.RectangleF (0, 0, 320, 30);
			TextLabel.Font = UIFont.SystemFontOfSize (UIFont.SmallSystemFontSize + 2);
			DetailTextLabel.Font = UIFont.SystemFontOfSize (UIFont.SmallSystemFontSize + 2);
		}

		#region ICellSelectable implementation

		public void OnSelect ()
		{
			if (Tapped != null)
				Tapped ();
		}

		#endregion
	}
}

