using System;

namespace WebApplication1.Models
{
	public class Customer
	{
		#region private properties

		#endregion

		#region Public properties

		public string CustomerID { get; set; }

		public int RecordID { get; set; }

		public string FirstName { get; set; }

		public string MiddleInitial { get; set; }

		public string LastName { get; set; }

		public string Company { get; set; }

		public string Address1 { get; set; }

		public string Address2 { get; set; }

		public string City { get; set; }

		public string Zip { get; set; }

		public string State { get; set; }

		public string Country { get; set; }

		public string HomePhone { get; set; }

		public string PhoneExt { get; set; }

		public string CellPhone { get; set; }

		public string Misc2 { get; set; }

		public string Email { get; set; }

		public string ShipName { get; set; }

		public string CellEmail { get; set; }

		public DateTime LastUpdate { get; set; }

		public double OnAccount { get; set; }

		public DateTime DateCreated { get; set; }
		public bool doesThisMatch(string inputString)
		{
			if (HomePhone == inputString)
			{
				return true;
			}
			if (CellPhone == inputString)
			{
				return true;
			}
			if (CustomerID == inputString)
			{
				return true;
			}
			return false;
		}

		#endregion
	}
}