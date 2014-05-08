using System;
using SQLite;

namespace iPadPos
{
	public class TaxType
	{
		[PrimaryKey]
		public string Id {get;set;}
		public string Description {get;set;}
		public double Rate {get;set;}
	}
}

