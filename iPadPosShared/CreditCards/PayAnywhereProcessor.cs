using System;
using System.Threading.Tasks;
using MonoTouch.UIKit;
using PayAnywhere;

namespace iPadPos
{
	public class PayAnywhereProcessor : CreditCardProcessor
	{
		public PayAnywhereProcessor ()
		{
		}

		#region implemented abstract members of CreditCardProcessor
		TaskCompletionSource<Tuple<ChargeDetails, string>> tcs;
		public override async Task<Tuple<ChargeDetails, string>> Charge (UIViewController parent, Invoice invoice)
		{
			tcs = new TaskCompletionSource<Tuple<ChargeDetails, string>> ();
			handler = new PATransactionHandler {
				Amount = invoice.CardPayment.Amount.ToString(),
				AppName = "Affinity",
				Invoice = invoice.Id,
				IsEmailOn = true,
				IsSignatureOn = true,
				IsSignatureRequired = true,
				LoginId = Settings.Shared.PayAnywhereLogin,
				MerchantId = Settings.Shared.PayAnywhereMerchantId,
				PassWord = Settings.Shared.PayAnywherePw,
				UserName = Settings.Shared.PayAnywhereUserId,
				TransactionType = Transactions.NewCharge,
				Delegate = new HandlerDelegate(tcs,invoice),
			};
			handler.Submit ();
			return await tcs.Task;
		}

		public override bool NeedsSignature {
			get {
				return false;
			}
		}

		#endregion
		PATransactionHandler handler;
		class HandlerDelegate : PATransactionHandlerDelegate
		{
			TaskCompletionSource<Tuple<ChargeDetails, string>> tcs;

			Invoice invoice;

			public HandlerDelegate(TaskCompletionSource<Tuple<ChargeDetails, string>> tcs, Invoice invoice)
			{
				this.invoice = invoice;
				this.tcs = tcs;

			}
			public override void Result (PayAnywhereResult response)
			{
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

