using System;
using SQLite;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

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
			CreateTable<TaxType> ();
			CreateTable<TransactionType> ();
			CreateTable<PaymentType> ();
			CreateTable<Customer> ();
			CreateTable<Item> ();
			CreateTable<ChargeDetails> ();

		}


		public async Task<Customer> GetCashCustomer()
		{
			var cust = Table<Customer>().Where(x=> x.CustomerId == Settings.Shared.CashCustomer).FirstOrDefault();
			if (cust == null) {
				cust = await WebService.Main.GetCustomer (Settings.Shared.CashCustomer);
				InsertOrReplace (cust);
			}
			return cust;
		}
	}
}

