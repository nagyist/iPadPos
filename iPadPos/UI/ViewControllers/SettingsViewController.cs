using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using iOSHelpers;
using System.Threading.Tasks;

namespace iPadPos
{
	public class SettingsViewController : DialogViewController
	{
		public SettingsViewController () : base (UITableViewStyle.Grouped, null)
		{
			var testButton = new SimpleButton {
				Title = "Test Connection",
				TitleColor = UIColor.Black,
				Tapped = async (t) =>{
					View.DismissKeyboard();
					var f =  t.Frame;
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
					new EntryElement ("Account Key", "acc_1dae92cb8808e3ce", Settings.Shared.CCAcountKey) {
						ShouldAutoCorrect = false,
						ValueUpdated = (v) => {
							Settings.Shared.CCAcountKey = v;
						},
					},
					new EntryElement ("Test Account Key", "acc_1dae92cb8808e3ce", Settings.Shared.TestCCAccountKey) {
						ShouldAutoCorrect = false,
						ValueUpdated = (v) => {
							Settings.Shared.TestCCAccountKey = v;
						},
					},
				},
			};
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

