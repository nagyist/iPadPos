using System;
using MonoTouch.UIKit;
using System.Threading.Tasks;
using SignaturePad;

namespace iPadPos
{
	public class SignatureViewController : UIViewController
	{
		public Action<bool> HasSignature { get; set; }
		public SignatureViewController ()
		{
			Title = "Please Sign";
			EdgesForExtendedLayout = UIRectEdge.None;
			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Done, (s, e) => {
				if(HasSignature != null)
					HasSignature(true);
				if(tcs != null)
					tcs.TrySetResult(true);
			});
			this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, (s, e) => {
				if(HasSignature != null)
					HasSignature(false);
				if(tcs != null)
					tcs.TrySetResult(false);
			});
		}
		public double Amount {
			set {
				view.Label.Text  = value.ToString("C");
			}
		}
		public override void LoadView ()
		{
			View = new SigView ();
		}
		SigView view { get { return View as SigView; } }
		public SignaturePadView SignatureView
		{
			get{ return view.SignaturePadView; }
		}
		TaskCompletionSource<bool> tcs;
		public async Task<bool> GetSignature ()
		{
			if (tcs == null)
				tcs = new TaskCompletionSource<bool> ();
			return await tcs.Task;
		}

		class SigView : UIView
		{
			public SignaturePadView SignaturePadView;
			public UILabel Label { get; set;}
			public SigView ()
			{
				BackgroundColor = Color.DarkGray;
				Add(Label = new UILabel{
					Font = UIFont.BoldSystemFontOfSize(60),
					TextAlignment = UITextAlignment.Center,
					TextColor = UIColor.White,
					Text = "$100",
				});
				Label.SizeToFit();
				Add(SignaturePadView = new SignaturePadView());
			}
			public override void LayoutSubviews ()
			{
				base.LayoutSubviews ();
				const float padding = 50f;
				var frame = Label.Frame;
				frame.X = 20;
				frame.Width = Bounds.Width - 40;
				frame.Y = padding;
				Label.Frame = frame;

				frame.Y = frame.Bottom + 10;
				frame.Height = Bounds.Height - (Frame.Y * 3);
				SignaturePadView.Frame = frame;

			}
		}
	}
}

