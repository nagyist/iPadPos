using System;
using CardFlightSdk;
using System.Threading.Tasks;
using MonoTouch.Foundation;


namespace iPadPos
{
	public class CreditCardProccessor
	{
		public CreditCardProccessor ()
		{
			#if DEBUG
			CardFlight.SharedInstance.Logging = true;
			#endif
			CardFlight.SharedInstance.Init ("1fc8e90a2a82f5b02b85ae7945535630", "acc_1dae92cb8808e3ce");
		}
		static CreditCardProccessor shared;
		public static CreditCardProccessor Shared {
			get {
				return shared ?? (shared = new CreditCardProccessor());
			}
			set {
				shared = value;
			}
		}
		MyReader reader;
		public async Task<ChargeDetails> Charge(Invoice invoice)
		{
			if (reader == null)
				reader = new MyReader ();
			if (!reader.IsConnected) {
				await reader.ConnectionTask.Task;
				Task.Delay (1000);
			}
			var card = await reader.Swipe();
			if (card == null)
				return null;
			var result = await card.ChargeAsync (invoice);
			if (result.Item1 == null) {
				Console.WriteLine (result.Item2);
				return null;
			}
			var charge = result.Item1;
			return new ChargeDetails{
				Amount = double.Parse(charge.Amount.ToString()),
				AmountRefunded = double.Parse(charge.AmountRefunded.ToString()),
				Created = charge.Created,
				IsRefunded = charge.IsRefunded,
				ReferenceID = charge.ReferenceID,
				Token = charge.Token,
			};
		}


		class MyReader : CFTReader
		{
			public bool IsConnected { get; set; }
			public TaskCompletionSource<bool> ConnectionTask = new TaskCompletionSource<bool>();
			public MyReader()
			{
				Delegate = new CardReaderDelegate(this);
				this.Connect();
			}
			TaskCompletionSource<Tuple<CFTCard,NSError>> swipeCompletion;
			public async Task<CFTCard> Swipe()
			{
				swipeCompletion = new TaskCompletionSource<Tuple<CFTCard, NSError>> ();
				BeginSwipeWithMessage ("Please swipe the card");
				var response = await swipeCompletion.Task;
				if (response.Item1 != null)
					return response.Item1;
				return null;
			}



			class CardReaderDelegate : CFTReaderDelegate
			{
				MyReader reader;

				public CardReaderDelegate(MyReader reader)
				{
					this.reader = reader;

				}

				public override void ReaderIsAttached ()
				{
					Console.WriteLine ("Reader is attached");
				}
				public override void CardResponse (CFTCard card, MonoTouch.Foundation.NSError error)
				{
					reader.swipeCompletion.TrySetResult(new Tuple<CFTCard,NSError>(card,error));
				}

				public override void GenericResponse (string cardData)
				{
					//Console.WriteLine (cardData);
					//reader.BeginSwipeWithMessage ("Please swipe the card");
					reader.swipeCompletion.TrySetResult(new Tuple<CFTCard,NSError>(null,new NSError()));
				}
//				public override void ReaderIsAttached ()
//				{
////					reader.IsConnected = false;
////					reader.ConnectionTask.TrySetResult (true);
////					Console.WriteLine ("Reader is attached");
//				}
				public override void ReaderIsConnecting ()
				{
					Console.WriteLine ("Reader connected");
				}
				public override void ReaderIsDisconnected ()
				{
					reader.IsConnected = false;
					reader.ConnectionTask.TrySetResult (false);
					Console.WriteLine ("Reader disconeected");
				}
				public override void ReaderSwipeDidCancel ()
				{
					Console.WriteLine ("SwipeDidCancel");
					reader.swipeCompletion.TrySetResult(new Tuple<CFTCard,NSError>(null,new NSError()));
				}
				public override void SerialNumber (string serialNumber)
				{
					Console.WriteLine ("Serial number {0}", serialNumber);
				}
				public override void IsConnected (bool isConnected, MonoTouch.Foundation.NSError error)
				{
					reader.IsConnected = isConnected;
					reader.ConnectionTask.TrySetResult (isConnected);
					Console.WriteLine ("Error");
					if(error != null)
						reader.swipeCompletion.TrySetResult(new Tuple<CFTCard,NSError>(null,error));
				}
			}
		}

	}
}

