using System;
using MonoTouch.UIKit;
using SignaturePad;
using System.Threading.Tasks;

namespace iPadPos
{
	public class SignatureViewController : UIViewController
	{
		public Action<bool> HasSignature { get; set; }
		public SignatureViewController ()
		{
			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Done, (s, e) => {
				if(HasSignature != null)
					HasSignature(true);
				if(tcs != null)
					tcs.TrySetResult(true);
			});
			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Done, (s, e) => {
				if(HasSignature != null)
					HasSignature(false);
				if(tcs != null)
					tcs.TrySetResult(false);
			});
		}
		public override void LoadView ()
		{
			View = new SignaturePadView ();
		}
		public SignaturePadView SignatureView
		{
			get{ return View as SignaturePadView; }
		}
		TaskCompletionSource<bool> tcs;
		public async Task<bool> GetSignature ()
		{
			if (tcs == null)
				tcs = new TaskCompletionSource<bool> ();
			return await tcs.Task;
		}
	}
}

