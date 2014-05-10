using System;
using MonoTouch.Foundation;
using SocketMobile;
using System.Collections.Generic;

namespace iPadPos
{

	public class SocketScannerHelper
	{
		static SocketScannerHelper shared;
		public static SocketScannerHelper Shared {
			get {
				return shared ?? (shared = new SocketScannerHelper());
			}
			set {
				shared = value;
			}
		}

		NSTimer timer;
		ScanApiHelper ScanApi;
		public SocketScannerHelper ()
		{
			if (MonoTouch.ObjCRuntime.Runtime.Arch == MonoTouch.ObjCRuntime.Arch.SIMULATOR)
				return;
			ScanApi = new ScanApiHelper ();
			ScanApi.Delegate = new ScanDelegate (this);
			ScanApi.Open ();
			timer = NSTimer.CreateRepeatingScheduledTimer (.2, () => {
				var r = ScanApi.DoScanApiReceive();
			});
		}

		public Action<string> Scaned { get; set; }

		public List<DeviceInfo> Devices = new List<DeviceInfo>();
		public class ScanDelegate : ScanApiHelperDelegate
		{
			SocketScannerHelper helper;

			public ScanDelegate (SocketScannerHelper helper)
			{
				this.helper = helper;
			}
			public override void Device (SKTRESULT result, DeviceInfo deviceInfo)
			{
				Console.WriteLine (deviceInfo.GetName);
				helper.Devices.Add (deviceInfo);
			}
			public override void DecodedData (DeviceInfo device, string decodedData)
			{
				Console.WriteLine (decodedData);
				helper.Scaned (decodedData.TrimEnd("\r".ToCharArray()));
			}
			public override void OnInitialized (SKTRESULT result)
			{
				Console.WriteLine (result);
			}
			public override void OnDeviceRemoval (DeviceInfo deviceRemoved)
			{
				Console.WriteLine (deviceRemoved.GetName);
				helper.Devices.Remove (deviceRemoved);
			}
		}
	}
}

