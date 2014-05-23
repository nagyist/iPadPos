using System;

namespace iPadPos
{
	public class Analytics
	{
		static Analytics shared;
		public static Analytics Shared {
			get {
				return shared ?? (shared = new Analytics());
			}
		}
		public Analytics ()
		{
			Xamarin.Analytics.Initialize ("3f3f968330e18e5af52fa87a009982a193c5ef84");
		}
		public void Init()
		{

		}

		public void Log (Exception ex)
		{
			Xamarin.Analytics.Report (ex);
		}
	}
}

