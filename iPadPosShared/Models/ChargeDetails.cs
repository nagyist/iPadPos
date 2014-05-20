using System;
using SQLite;

namespace iPadPos
{
	public class ChargeDetails
	{
		public double Amount { get; set;}

		[PrimaryKey]
		public string Token { get; set;}

		public string ReferenceID { get; set;}

		public bool IsRefunded { get; set;}

		public double AmountRefunded { get; set;}

		public DateTime Created { get; set;}

		[Newtonsoft.Json.JsonIgnore]
		public int LocalInvoiceId {get;set;}
	}
}

