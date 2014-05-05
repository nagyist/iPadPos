using System;
using SQLite;
using System.IO;

namespace iPadPos
{
	public class Database : SQLiteConnection
	{
		public static string MainDbPath = Path.Combine(Locations.LibDir,"main.db");
		static Database main;
		public static Database Main { get { return main ?? (main = new Database()); }}
		//public Database () : base(MainDbPath,"em@p55",true)
		public Database () : base(MainDbPath,true)
		{
			CreateTable<Invoice> ();
			CreateTable<InvoiceLine>();
		}
	}
}

