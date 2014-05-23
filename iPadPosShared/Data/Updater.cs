using System;
using System.Runtime.InteropServices;
using HockeyApp;
using MonoTouch.AdSupport;
using MonoTouch.Foundation;

namespace iPadPos
{
	public class Updater
	{
		static Updater shared;
		public static Updater Shared {
			get {
				return shared ?? (shared = new Updater());
			}
		}
		public Updater ()
		{
		}
		public void Init()
		{
			//#if !DEBUG
			EnableCrashReporting ();
			//#endif
		}
		[DllImport ("libc")]
		private static extern int sigaction (Signal sig, IntPtr act, IntPtr oact);

		enum Signal {
			SIGBUS = 10,
			SIGSEGV = 11
		}

		/// <summary>
		/// This method works around a problem with all iOS crash reporters.
		/// 
		/// Because they override signal handlers for SIGSEGV and SIGBUS,
		/// they break null reference exception handling in Mono.
		/// 
		/// We have to re-install Mono signal handlers.
		/// 
		/// Read more about this fix here:
		/// http://stackoverflow.com/a/14499336/458193
		/// </summary>
		static void EnableCrashReporting ()
		{
			IntPtr sigbus = Marshal.AllocHGlobal (512);
			IntPtr sigsegv = Marshal.AllocHGlobal (512);

			// Store Mono SIGSEGV and SIGBUS handlers
			sigaction (Signal.SIGBUS, IntPtr.Zero, sigbus);
			sigaction (Signal.SIGSEGV, IntPtr.Zero, sigsegv);

			try{
			// Enable crash reporting libraries
				EnableCrashReportingUnsafe ();
			}
			catch(Exception ex) {
				Console.WriteLine (ex);
				Analytics.Shared.Log (ex);
			}

			// Restore Mono SIGSEGV and SIGBUS handlers            
			sigaction (Signal.SIGBUS, sigbus, IntPtr.Zero);
			sigaction (Signal.SIGSEGV, sigsegv, IntPtr.Zero);
		}

		static void EnableCrashReportingUnsafe ()
		{
			// Run your crash reporting library initialization code here--
			// this example uses HockeyApp but it should work well
			// with TestFlight or other libraries.

			// Verify in documentation that your library of choice
			// installs its sigaction hooks before leaving this method.

			var manager = BITHockeyManager.SharedHockeyManager;
			manager.DisableCrashManager = true;
			Console.WriteLine (manager.AppStoreEnvironment);
			manager.Configure ("d396844720672ffa11ded8c7a66175fd", null);
			manager.UpdateManager.UpdateSetting = BITUpdateSetting.CheckStartup;
			manager.EnableStoreUpdateManager = true;
			manager.UpdateManager.AlwaysShowUpdateReminder = true;
			manager.UpdateManager.CheckForUpdateOnLaunch = true;
			manager.StartManager ();
		}	
		public void Update()
		{
			BITHockeyManager.SharedHockeyManager.UpdateManager.ShowUpdateView ();
		}

	

	}
}

