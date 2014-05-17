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
				NSNumber.FromDouble(invoice.CardPayment.Amount),
//				new NSString("Anchorage KiD 2 KiD")
			}, new [] {
				new NSString("amount"),
//				new NSString("description")
			});
			var tcs = new TaskCompletionSource<Tuple<CFTCharge,string>>();
			card.ChargeCard (dict, (charge) =>  tcs.TrySetResult (new Tuple<CFTCharge, string> (charge, "")), (error) => {

				tcs.TrySetResult(new Tuple<CFTCharge, string> (null, error.ToString ()));
			});
			return tcs.Task;
		}
	}
}

