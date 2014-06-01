using System;
using System.Threading.Tasks;
using MonoTouch.UIKit;
using PayAnywhere;
using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using System.Collections.Generic;
using System.Linq;

namespace iPadPos
{

	public class PayAnywhereProcessor : CreditCardProcessor
	{

		public PayAnywhereProcessor ()
		{
			SetUpImages ();
		}

		#region implemented abstract members of CreditCardProcessor
		TaskCompletionSource<Tuple<ChargeDetails, string>> tcs;
		public override async Task<Tuple<ChargeDetails, string>> Charge (UIViewController parent, Invoice invoice)
		{
			tcs = new TaskCompletionSource<Tuple<ChargeDetails, string>> ();

			PATransactionHandler.DataHolder.Delegate = new HandlerDelegate (tcs, invoice);

			PATransactionHandler.DataHolder.AppName = "Affinity";
			PATransactionHandler.DataHolder.LoginId = Settings.Shared.PayAnywhereLogin;
			PATransactionHandler.DataHolder.MerchantId = Settings.Shared.PayAnywhereMerchantId;
			PATransactionHandler.DataHolder.PassWord = Settings.Shared.PayAnywherePw;
			PATransactionHandler.DataHolder.UserName = Settings.Shared.PayAnywhereUserId;

			PATransactionHandler.DataHolder.Amount = invoice.CardPayment.Amount.ToString ();
			PATransactionHandler.DataHolder.Invoice = invoice.Id;
			PATransactionHandler.DataHolder.IsEmailOn = true;
			PATransactionHandler.DataHolder.IsSignatureOn = true;
			PATransactionHandler.DataHolder.IsSignatureRequired = true;

			PATransactionHandler.DataHolder.TransactionType = Transactions.NewCharge;

			PATransactionHandler.DataHolder.Submit ();
			return await tcs.Task;
		}

		public void SetUpImages()
		{

			PATransactionHandler.DataHolder.SupportedOrientations = new [] {
				NSNumber.FromInt32 ((int)UIInterfaceOrientation.LandscapeLeft),
				NSNumber.FromInt32 ((int)UIInterfaceOrientation.LandscapeRight),

			};

//			PATransactionHandler.DataHolder.BackgroundImage = UIImage.FromBundle (imageName("background"));
//			PATransactionHandler.DataHolder.BackgroundImageLandscape = UIImage.FromBundle (imageName ("background_landscape"));
			PATransactionHandler.DataHolder.LogoImage = UIImage.FromBundle (imageName("merchant_logo"));
			PATransactionHandler.DataHolder.LogoImageLandscape = UIImage.FromBundle (imageName("merchant_logo_landscape"));
			PATransactionHandler.DataHolder.BackButtonImage = UIImage.FromBundle (imageName("back"));
			PATransactionHandler.DataHolder.SwipeCardImage = UIImage.FromBundle (imageName("swipeCard"));
			PATransactionHandler.DataHolder.ManuallyEnterImage = null;
//			PATransactionHandler.DataHolder.ChargeButtonImage = UIImage.FromBundle (imageName("charge"));
			PATransactionHandler.DataHolder.ProcessingImage = UIImage.FromBundle (imageName("processing"));
			PATransactionHandler.DataHolder.ApprovedImage = UIImage.FromBundle (imageName("approved"));
			PATransactionHandler.DataHolder.DeclinedImage = UIImage.FromBundle (imageName("declined"));
			PATransactionHandler.DataHolder.OkButtonImage = UIImage.FromBundle (imageName("ok"));
			PATransactionHandler.DataHolder.EmailButtonImage = UIImage.FromBundle (imageName("emailReceipt"));
			//PATransactionHandler.DataHolder.EmailButtonImageLandscape = UIImage.FromBundle (imageName("emailReceiptShort"));
			PATransactionHandler.DataHolder.NoThanksButtonImage = UIImage.FromBundle (imageName("noThanks"));
			//PATransactionHandler.DataHolder.NoThanksButtonImageLandscape = UIImage.FromBundle (imageName("noThanksShort"));

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {

				var keypad = Enumerable.Range (1, 9).Select (x => UIImage.FromBundle (string.Format ("0{0}", x))).ToList ();

				keypad.Add (UIImage.FromBundle ("00"));
				keypad.Add (UIImage.FromBundle ("0"));
				keypad.Add (UIImage.FromBundle ("delete"));
				PATransactionHandler.DataHolder.KeyPadArrayImages = keypad.ToArray ();
				//        NSMutableArray *keyPadArrayImages = [[NSMutableArray alloc] init];
				//        for (int i = 1; i < 10; i++)
				//            [keyPadArrayImages addObject:[UIImage imageNamed:[NSString stringWithFormat:@"0%d.png",i]]];
				//        [keyPadArrayImages addObject:[UIImage imageNamed:@"00.png"]];
				//        [keyPadArrayImages addObject:[UIImage imageNamed:@"0.png"]];
				//        [keyPadArrayImages addObject:[UIImage imageNamed:@"delete.png"]];
				//        [[PATransactionHandler dataHolder] setKeyPadArrayImages:keyPadArrayImages];
				//        [keyPadArrayImages release];
				//    }
			}
		}
		string imageName(string name)
		{
			var imagename = string.Format("{0}{1}.png",name,UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone ? "" : "_iPad");
			return imagename;
		}
		public override bool NeedsSignature {
			get {
				return false;
			}
		}

		#endregion
		class HandlerDelegate : PATransactionHandlerDelegate
		{
			TaskCompletionSource<Tuple<ChargeDetails, string>> tcs;

			Invoice invoice;

			public HandlerDelegate(TaskCompletionSource<Tuple<ChargeDetails, string>> tcs, Invoice invoice)
			{
				this.invoice = invoice;
				this.tcs = tcs;

			}
			public override void Result (NSMutableDictionary r)
			{
				var response = (PayAnywhereResult)r;
				if (response.Status == TransactionStatus.Approved) {
					tcs.TrySetResult (new Tuple<ChargeDetails, string> (new ChargeDetails{
						Amount = invoice.CardPayment.Amount,
						AmountRefunded = 0,
						Created = DateTime.Now,
						IsRefunded = false,
						LocalInvoiceId = invoice.LocalId,
						ReferenceID = response.ReferenceNumber,
						Token = response.ReferenceNumber,
						
					},"Success"));
					return;
				}

				tcs.TrySetResult (new Tuple<ChargeDetails, string> (null, response.Status == TransactionStatus.Canceled ? "The transaction was canceled" : "The card was declined"));
			}
		}
	}
}

