using System;
using System.Threading.Tasks;

namespace iPadPos
{
	public class PaypalProcessor : CreditCardProcessor
	{
		public PaypalProcessor()
		{
			this.ProcessorType = CreditCardProcessorType.Paypal;
		}


		public async Task ProcessCallback(string url)
		{

		}
		public override bool NeedsSignature {
			get {
				return false;
			}
		}
		#region implemented abstract members of InvoiceProcessor
		TaskCompletionSource<Tuple<ChargeDetails, string>> tcs;
		public override async Task<Tuple<ChargeDetails, string>> Charge (Invoice invoice)
		{
			return new Tuple<ChargeDetails, string> (null, "Unknown Error");
		}

		#endregion
	}
}

