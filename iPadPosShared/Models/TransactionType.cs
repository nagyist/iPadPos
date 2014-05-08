using System;
using SQLite;

namespace iPadPos
{
	public class TransactionType
	{
		[PrimaryKey]
		public string Id {get;set;}
		public string Description {get;set;}
		public int Multiplier {get;set;}
	}
}

