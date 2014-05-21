using System;
using SQLite;
using System.ComponentModel;
using MonoTouch.Foundation;

namespace iPadPos
{
	public class Settings : BaseModel
	{
		public const string LastPostedChangeKey = "LastPostedChange";
		public const string LastPostedInvoiceKey = "LastPostedInvoice";
		public const string HasDataKey = "HadData";

		static Settings shared;
		public static Settings Shared {
			get {
				return shared ?? (shared = new Settings());
			}
		}

		public Settings ()
		{
			Database.Main.CreateTable<Setting> ();
		}
		public string CurrentServerUrl
		{
			get{ return TestMode ? TestServerUrl : ServerUrl; }
		}

		public string ServerUrl
		{
			get{
				string apiUri = NSUserDefaults.StandardUserDefaults.StringForKey ("Server");
				if (!string.IsNullOrEmpty (apiUri) && apiUri.EndsWith ("/"))
					apiUri += "/";

				return apiUri ?? "http://clancey.dyndns.org:32021/api/";
			}
			set{
				NSUserDefaults.StandardUserDefaults.SetValueForKey ((NSString)value, (NSString)"Server");
				NSUserDefaults.StandardUserDefaults.Synchronize ();
				ProcPropertyChanged ("ServerUrl");
			}
		}
		public string TestServerUrl
		{
			get{
				string apiUri = NSUserDefaults.StandardUserDefaults.StringForKey ("TestServer");
				if (!string.IsNullOrEmpty (apiUri) && apiUri.EndsWith ("/"))
					apiUri += "/";
				return apiUri  ?? "http://clancey.dyndns.org:32021/api/";
			}
			set{
				NSUserDefaults.StandardUserDefaults.SetValueForKey ((NSString)value, (NSString)"TestServer");
				NSUserDefaults.StandardUserDefaults.Synchronize ();
				ProcPropertyChanged ("TestServerUrl");
			}
		}
		public bool TestMode
		{
			get{return NSUserDefaults.StandardUserDefaults.BoolForKey ("TestMode");}
			set{ 
				NSUserDefaults.StandardUserDefaults.SetValueForKey (NSNumber.FromBoolean(value),(NSString)"TestMode");
				NSUserDefaults.StandardUserDefaults.Synchronize ();
				ProcPropertyChanged ("TestMode");
			}
		}
	

		public double LastPostedChange {
			get { return GetDouble (LastPostedChangeKey); }
			set { 
				SetValue (LastPostedChangeKey, value);
				ProcPropertyChanged ("LastPostedChangeString");
			}
		}

		public string LastPostedInvoice {
			get { return GetStringValue (LastPostedInvoiceKey); }
			set { 
				SetValue (LastPostedInvoiceKey, value);
			}
		}
		public string LastPostedChangeString
		{
			get{ return LastPostedChange.ToString ("C"); }
			set{ LastPostedChange = string.IsNullOrEmpty(value) ? 0 : double.Parse (value, System.Globalization.NumberStyles.Currency); }
		}

		public bool HasData
		{
			get{ return GetBool (HasDataKey); }
			set{ SetValue (HasDataKey, value); }
		}

		public int CurrentInvoice {
			get { 
				return GetInt ("CurrentInvoice"); }
			set {
				SetValue ("CurrentInvoice", value);
			}
		}
		public string CashCustomer
		{
			get { return GetStringValue ("CashCustomerId") ?? "CASH"; }
			set { 
				SetValue ("CashCustomerId", value);
			}
		}

		public int RegisterId
		{
			get{ return Math.Max(GetInt ("RegisterId"),1); }
			set{ SetValue ("RegisterId", value); }
		}


		public string GetStringValue(string key)
		{
			var setting = Database.Main.Table<Setting> ().Where (x => x.Key == key).FirstOrDefault ();
			return setting == null ? null : setting.Value;
		}

		public double GetDouble(string key)
		{
			double value;
			return double.TryParse (GetStringValue (key), out value) ? value : 0;
		}

		public int GetInt(string key)
		{
			int value;
			return int.TryParse (GetStringValue (key), out value) ? value : 0;
		}

		public bool GetBool(string key)
		{
			bool value;
			return bool.TryParse (GetStringValue (key), out value) && value;
		}

		public void SetValue(string key, string value)
		{
			var oldValue = GetStringValue (key);
			Database.Main.InsertOrReplace (new Setting{ Key = key, Value = value });
			if (oldValue != value)
				ProcPropertyChanged (key);
		}
		public void SetValue(string key, int value)
		{
			SetValue (key, value.ToString ());
		}

		public void SetValue(string key, object value)
		{
			SetValue (key, value.ToString ());
		}
//		public void SetValue(string key, bool value)
//		{
//			SetValue (key, value.ToString ());
//		}


		class Setting
		{
			[PrimaryKey]
			public string Key {get;set;}

			public string Value {get;set;}
		}
	}
}

