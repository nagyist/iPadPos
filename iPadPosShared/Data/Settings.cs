using System;
using SQLite;
using System.ComponentModel;

namespace iPadPos
{
	public class Settings : BaseModel
	{

		public const string CurrentRegionKey = "CurrentRegionId";
		public const string CurrentCountryKey = "CurrentCountryId";
		public const string CurrentContinentKey = "CurrentContinentId";
		public const string CurrentRegionTypeKey = "CurrentRegionTypeId";
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

		public int CurrentRegionId{ 
			get { return GetInt (CurrentRegionKey); }
			set { SetValue (CurrentRegionKey, value); }
		}

		public int CurrentRegiontype {
			get{ return GetInt (CurrentRegionTypeKey); }
			set{ SetValue (CurrentRegionTypeKey, value); }
		}

		public int CurrentCountry {
			get { return GetInt (CurrentCountryKey); }
			set { SetValue (CurrentCountryKey, value); }
		}

		public int CurrentContinent {
			get { return GetInt (CurrentContinentKey); }
			set { SetValue (CurrentContinentKey, value); }
		}

		public bool HasData
		{
			get{ return GetBool (HasDataKey); }
			set{ SetValue (HasDataKey, value); }
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

