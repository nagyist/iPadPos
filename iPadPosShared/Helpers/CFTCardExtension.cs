using System;
using System.Threading.Tasks;
using CardFlightSdk;
using MonoTouch.Foundation;

namespace iPadPos
{
	public static class CFTCardExtension
	{
		public static Task<Tuple<CFTCharge,string>> ChargeAsync(this CFTCard card, Invoice invoice)
		{
			var dict = NSDictionary.FromObjectsAndKeys (new object[] {
				new NSDecimalNumber(invoice.CardPayment.Amount.ToString()),
			}, new [] {
				new NSString("amount"),
			});

			var tcs = new TaskCompletionSource<Tuple<CFTCharge,string>>();
			Console.WriteLine (dict);
			card.ChargeCard (dict, (charge) =>  tcs.TrySetResult (new Tuple<CFTCharge, string> (charge, "")), (error) => tcs.TrySetResult (new Tuple<CFTCharge, string> (null, error.ToString ())));
			return tcs.Task;
		}
	}
}

