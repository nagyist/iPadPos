using System;
using SQLite;

namespace iPadPos
{
	public class PaymentType
	{
		[PrimaryKey]
		public string Id { get; set; }
		public string Description { get; set; }
		public bool IsActive { get; set; }
		public bool HasExtra { get; set; }
		public string LaunchAppUrl { get; set; }
		public int SortOrder { get; set; }
	}
}

