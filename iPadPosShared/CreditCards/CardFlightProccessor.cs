//using System;
//using CardFlightSdk;
//using System.Threading.Tasks;
//using MonoTouch.Foundation;
//using MonoTouch.UIKit;
//
//
//namespace iPadPos
//{
//	public class CardFlightProccessor : CreditCardProcessor
//	{
//		public CardFlightProccessor ()
//		{
//			this.ProcessorType = CreditCardProcessorType.CardFlight;
//			#if DEBUG
//			CardFlight.SharedInstance.Logging = true;
//			#endif
//			Init ();
//			Settings.Shared.SubscribeToProperty ("CCAcountKey", Init);
//			Settings.Shared.SubscribeToProperty ("TestMode", Init);
//
//		}
//		void Init()
//		{
//			var api = ApiKey;
//			var acc = Settings.Shared.CurrentCCAcountKey;
//			CardFlight.SharedInstance.Init (api, acc);
//		}
//		public override bool NeedsSignature {
//			get {
//				return true;
//			}
//		}
//		static string ApiKey
//		{
//			get{ return Settings.Shared.TestMode ? "1fc8e90a2a82f5b02b85ae7945535630" : "269a6e69b827b9e8229c0654f85e1ee2"; }
//		}
//
//		MyReader reader;
//		public override async Task<Tuple<ChargeDetails,string>> Charge(UIViewController parent,Invoice invoice)
//		{
//			if (MonoTouch.ObjCRuntime.Runtime.Arch == MonoTouch.ObjCRuntime.Arch.SIMULATOR) {
//				return new Tuple<ChargeDetails, string>( new ChargeDetails{
//					Amount = invoice.Total,
//					Created = DateTime.Now,
//					IsRefunded = false,
//					ReferenceID = "SimTransaction",
//					Token = string.Format("SimTest-{0}",DateTime.Now),
//				},"");
//			}
//			if (reader == null)
//				reader = new MyReader ();
//			if (!reader.IsConnected) {
//				await reader.ConnectionTask.Task;
//				await Task.Delay (1000);
//			}
//			var card = await reader.Swipe();
//			if (card.Item1 == null)
//				return new Tuple<ChargeDetails, string>(null,card.Item2.ToString());
//			var result = await card.Item1.ChargeAsync (invoice);
//			if (result.Item1 == null) {
//				Console.WriteLine (result.Item2);
//				return new Tuple<ChargeDetails, string>(null,result.Item2.ToString());
//			}
//			var charge = result.Item1;
//			return new Tuple<ChargeDetails, string>( new ChargeDetails{
//				Amount = double.Parse(charge.Amount.ToString()),
//				AmountRefunded = double.Parse(charge.AmountRefunded.ToString()),
//				Created = charge.Created,
//				IsRefunded = charge.IsRefunded,
//				ReferenceID = charge.ReferenceID,
//				Token = charge.Token,
//			},"");
//		}
//
//
//		class MyReader : CFTReader
//		{
//			public bool IsConnected { get; set; }
//			public TaskCompletionSource<bool> ConnectionTask = new TaskCompletionSource<bool>();
//			public MyReader()
//			{
//				Delegate = new CardReaderDelegate(this);
//				this.Connect();
//			}
//			TaskCompletionSource<Tuple<CFTCard,NSError>> swipeCompletion;
//			public async Task<Tuple<CFTCard, NSError>> Swipe()
//			{
//				swipeCompletion = new TaskCompletionSource<Tuple<CFTCard, NSError>> ();
//				BeginSwipeWithMessage ("Please swipe the card");
//				var response = await swipeCompletion.Task;
//				return response;
//			}
//
//
//
//			class CardReaderDelegate : CFTReaderDelegate
//			{
//				MyReader reader;
//
//				public CardReaderDelegate(MyReader reader)
//				{
//					this.reader = reader;
//
//				}
//
//				public override void ReaderIsAttached ()
//				{
//					Console.WriteLine ("Reader is attached");
//				}
//				public override void CardResponse (CFTCard card, MonoTouch.Foundation.NSError error)
//				{
//					reader.swipeCompletion.TrySetResult(new Tuple<CFTCard,NSError>(card,error));
//				}
//
//				public override void GenericResponse (string cardData)
//				{
//					//Console.WriteLine (cardData);
//					//reader.BeginSwipeWithMessage ("Please swipe the card");
//					reader.swipeCompletion.TrySetResult(new Tuple<CFTCard,NSError>(null,new NSError()));
//				}
////				public override void ReaderIsAttached ()
////				{
//////					reader.IsConnected = false;
//////					reader.ConnectionTask.TrySetResult (true);
//////					Console.WriteLine ("Reader is attached");
////				}
//				public override void ReaderIsConnecting ()
//				{
//					Console.WriteLine ("Reader connected");
//				}
//				public override void ReaderIsDisconnected ()
//				{
//					reader.IsConnected = false;
//					reader.ConnectionTask.TrySetResult (false);
//					Console.WriteLine ("Reader disconeected");
//				}
//				public override void ReaderSwipeDidCancel ()
//				{
//					Console.WriteLine ("SwipeDidCancel");
//					reader.swipeCompletion.TrySetResult(new Tuple<CFTCard,NSError>(null,new NSError()));
//				}
//				public override void SerialNumber (string serialNumber)
//				{
//					Console.WriteLine ("Serial number {0}", serialNumber);
//				}
//				public override void IsConnected (bool isConnected, MonoTouch.Foundation.NSError error)
//				{
//					reader.IsConnected = isConnected;
//					reader.ConnectionTask.TrySetResult (isConnected);
//					Console.WriteLine ("Error");
//					if(error != null)
//						reader.swipeCompletion.TrySetResult(new Tuple<CFTCard,NSError>(null,error));
//				}
//			}
//		}
//
//	}
//}
//
