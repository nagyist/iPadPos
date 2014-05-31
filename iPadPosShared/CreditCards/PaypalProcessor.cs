using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
using MonoTouch.UIKit;

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
			var uri = new Uri (url);
			var success = HttpUtility.ParseQueryString (uri.Query).Get ("Type").ToLower () != "cancel";
			var id = HttpUtility.ParseQueryString (uri.Query).Get ("InvoiceId");
			if (success)
				tcs.TrySetResult (new Tuple<ChargeDetails, string> (new ChargeDetails {
					Amount = invoice.CardPayment.Amount,
					AmountRefunded = 0,
					Created = DateTime.Now,
					IsRefunded = false,
					LocalInvoiceId = invoice.LocalId,
					ReferenceID = id,
					Token = id,
				}, ""));
			else
				tcs.TrySetResult (new Tuple<ChargeDetails, string> (null, "Canceled")); 
		}
		public override bool NeedsSignature {
			get {
				return false;
			}
		}
		Invoice invoice;
		TaskCompletionSource<Tuple<ChargeDetails, string>> tcs;
		public override async Task<Tuple<ChargeDetails, string>> Charge (UIViewController parent,Invoice invoice)
		{
			this.invoice = invoice;
			if(!App.CanOpenUrl("paypalhere://takePayment"))
			{
				return new Tuple<ChargeDetails, string> (null, "Paypal Here not installed");
			}
			else if(string.IsNullOrEmpty(Settings.Shared.PaypalId))
				return new Tuple<ChargeDetails, string> (null, "Please set the paypal email in the settings");
			var paypal = new PaypalRoot{
				merchantEmail = Settings.Shared.PaypalId,
				currencyCode = "USD",
				paymentTerms = "DueOnReceipt",

				itemList = new ItemList{
					item = {
						new PaypalItem{
							name = "KiD to KiD of Anchorage",
							description = "Payment for invoice " + invoice.Id,
							quantity = "1",
							unitPrice = invoice.CardPayment.Amount.ToString()
						}
					}
				}
			};
			var returnUrl = HttpUtility.UrlEncode("myapp://paypalHandler?{result}?Type={Type}&InvoiceId={InvoiceId}&Tip={Tip}&Email={Email}&TxId={TxId}");
			var invoiceJson = Newtonsoft.Json.JsonConvert.SerializeObject (paypal);
			var url = string.Format ("paypalhere://takePayment?accepted=card,paypal&returnUrl={0}&invoice={1}&step=choosePayment",returnUrl,invoiceJson);


			tcs = new TaskCompletionSource<Tuple<ChargeDetails, string>> ();
			if(App.OpenUrl (url))
				return await tcs.Task;

			return new Tuple<ChargeDetails, string> (null, "Unknown Error");
		}

		public class PaypalItem
		{
			public string name { get; set; }
			public string description { get; set; }
			public string quantity { get; set; }
			public string unitPrice { get; set; }
			public string taxName { get; set; }
			public string taxRate { get; set; }
			public string InvoiceId {get;set;}
		}

		public class ItemList
		{
			public List<PaypalItem> item { get; set; }
		}

		public class PaypalRoot
		{
			public string merchantEmail { get; set; }
			public string payerEmail { get; set; }
			public ItemList itemList { get; set; }
			public string currencyCode { get; set; }
			public string paymentTerms { get; set; }
			public string discountPercent { get; set; }
		}

	}
}

