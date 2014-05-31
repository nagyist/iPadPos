using System;
using System.Threading.Tasks;

namespace iPadPos
{
	public enum CreditCardProcessorType
	{
		CardFlight,
		Paypal,
	}
	public abstract class CreditCardProcessor
	{

		static CreditCardProcessor shared;
		public static CreditCardProcessor Shared {
			get {
				if (shared == null) {
					switch (Settings.Shared.CreditCardProcessor) {
					case CreditCardProcessorType.CardFlight:
						shared = new CardFlightProccessor ();
						return shared;
					case CreditCardProcessorType.Paypal:
						shared = new PaypalProcessor ();
						return shared;
					}
				}
				return shared;
			}
			set {
				shared = value;
			}
		}

		public CreditCardProcessorType ProcessorType {get;set;}
		public abstract Task<Tuple<ChargeDetails,string>> Charge(Invoice invoice);

		public abstract bool NeedsSignature {get;}
	}

}

