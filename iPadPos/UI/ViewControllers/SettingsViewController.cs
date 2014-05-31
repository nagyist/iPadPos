using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using iOSHelpers;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using System.Linq;

namespace iPadPos
{
	public class SettingsViewController : DialogViewController
	{
		Section paymentSection;
		StringElement processorType;
		public SettingsViewController () : base (UITableViewStyle.Grouped, null)
		{

			var testButton = new SimpleButton {
				Title = "Test Connection",
				TitleColor = UIColor.Black,
				Tapped = async (t) =>{
					View.DismissKeyboard();
					var f =  t.Frame;
					t.Title = "Testing...";
					t.Title = string.Format("Test Connection: {0}", await WebService.Main.Test());
					t.Frame = f;
				}
			};
			this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Done, (s, e) => save ());
			Root = new RootElement ("Settings") {
				new Section ("Server Settings") {
					new EntryElement ("Server", "http://10.0.1.2/api/", Settings.Shared.ServerUrl) {
						ShouldAutoCorrect = false,
						ValueUpdated = (v) => {
							Settings.Shared.ServerUrl = v;
						},
					},
					new EntryElement ("Test Server", "http://10.0.1.2/api/", Settings.Shared.TestServerUrl) {
						ShouldAutoCorrect = false,
						ValueUpdated = (v) => {
							Settings.Shared.TestServerUrl = v;
						},
					},
					new UIViewElement("",testButton,false),
				},
				new Section ("iPad Settings") {
					new BooleanElement("Test Mode",Settings.Shared.TestMode){
						ValueUpdated = (v) =>{
							Settings.Shared.TestMode = v;
						}
					},
					new EntryElement ("Register Id", "1", Settings.Shared.RegisterId.ToString()) {
						ShouldAutoCorrect = false,
						ValueUpdated = (v) => {
							try {
								Settings.Shared.RegisterId = int.Parse (v);
							} catch (Exception ex) {
								Console.WriteLine(ex);
								new SimpleAlertView ("Invalid Register ID", "The Register ID must be a number").Show ();
							}
						},
					},

				},
				new Section ("Payment Settings") {
					(processorType = new StringElement("Credit Card Processor",Settings.Shared.CreditCardProcessor.ToString(),()=>{
						//
						var sheet = new SimpleActionSheet();
						Enum.GetValues(typeof(CreditCardProcessorType)).Cast<CreditCardProcessorType>().ToList().ForEach(x=> sheet.Add(x.ToString(),Color.LightBlue,()=> {
							if(x == CreditCardProcessorType.Paypal)
							{
								//check if paypal is installed
							}
							processorType.Value = x.ToString();
							Settings.Shared.CreditCardProcessor = x;
							processorType.Reload();
							UpdatePaymentDetails();
						}));
						sheet.ShowFrom(processorType.GetActiveCell().Bounds,processorType.GetActiveCell(),true);
					})),

				},
				new Section(){
					new StringElement("Version",NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString()),
					new StringElement("Check for updates",() => Updater.Shared.Update()), 
				}
			};
			UpdatePaymentDetails ();
		}

		void UpdatePaymentDetails()
		{
			if(paymentSection != null)
				Root.Remove (paymentSection);
//			if (Settings.Shared.CreditCardProcessor == CreditCardProcessorType.CardFlight) {
//				paymentSection = new Section ("Cardflight details") {
//					new EntryElement ("Account Key", "acc_1dae92cb8808e3ce", Settings.Shared.CCAcountKey) {
//						ShouldAutoCorrect = false,
//						ValueUpdated = (v) => {
//							Settings.Shared.CCAcountKey = v;
//						},
//					},
//					new EntryElement ("Test Account Key", "acc_1dae92cb8808e3ce", Settings.Shared.TestCCAccountKey) {
//						ShouldAutoCorrect = false,
//						ValueUpdated = (v) => {
//							Settings.Shared.TestCCAccountKey = v;
//						},
//					},
//				};
//				Root.Insert (3, paymentSection);
//			}
			if (Settings.Shared.CreditCardProcessor == CreditCardProcessorType.Paypal) {
				paymentSection = new Section ("Paypal details") {
					new EntryElement ("Email", "Paypal email address", Settings.Shared.PaypalId) {
						ShouldAutoCorrect = false,
						KeyboardType = UIKeyboardType.EmailAddress,
						ValueUpdated = (v) => {
							Settings.Shared.PaypalId = v;
						},
					}
				};
				if (!App.HasPaypal ()) {
					var testButton = new SimpleButton {
						Title = "Install Paypal",
						TitleColor = UIColor.Black,
						Tapped = async (t) => {
							View.DismissKeyboard ();

							App.OpenUrl ("itms://itunes.apple.com/us/app/paypal-here/id505911015?mt=8");
						}
					};
					paymentSection.Add (new UIViewElement ("", testButton, false));
				}
				Root.Insert (3, paymentSection);
			} else if (Settings.Shared.CreditCardProcessor == CreditCardProcessorType.PayAnywhere) {
				paymentSection = new Section ("PayAnywhere details") {
					new EntryElement ("MerchantId", "", Settings.Shared.PayAnywhereMerchantId) {
						ShouldAutoCorrect = false,
						ValueUpdated = (v) => {
							Settings.Shared.PayAnywhereMerchantId = v;
						},
					},
					new EntryElement ("Login", "", Settings.Shared.PayAnywhereLogin) {
						ShouldAutoCorrect = false,
						ValueUpdated = (v) => {
							Settings.Shared.PayAnywhereLogin = v;
						},
					},
					new EntryElement ("UserName", "", Settings.Shared.PayAnywhereUserId) {
						ShouldAutoCorrect = false,
						ValueUpdated = (v) => {
							Settings.Shared.PayAnywhereUserId = v;
						},
					},
					new EntryElement ("Password", "", Settings.Shared.PayAnywherePw) {
						ShouldAutoCorrect = false,
						ValueUpdated = (v) => {
							Settings.Shared.PayAnywherePw = v;
						},
					},
				};

				Root.Insert (3, paymentSection);
			}
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			UpdatePaymentDetails ();
		}


		void save ()
		{
			View.DismissKeyboard ();
			DismissViewControllerAsync (true);
			if (saveTask != null)
				saveTask.TrySetResult (true);
		}
		TaskCompletionSource<bool> saveTask;
		public async Task Saved()
		{
			saveTask = new TaskCompletionSource<bool> ();

			await saveTask.Task; 
		}
	}
}

