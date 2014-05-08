using System;

namespace iPadPos
{
	public class Customer
	{
		public string CustomerId { get; set; }

		public string Company { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string MiddleName { get; set; }

		public string Address { get; set; }

		public string Address2 { get; set; }

		public string City { get; set; }

		public string State { get; set; }

		public string Zip { get; set; }

		public string Country { get; set; }

		public string Phone { get; set; }

		public string Misc1 { get; set; }

		public string Misc2 { get; set; }

		public DateTime LastUpdate { get; set; }

		public DateTime DateCreated { get; set; }

		public string Email { get; set; }

		public double OnAccount { get; set;}

		public override string ToString ()
		{
			return string.Format ("{0} {1}",FirstName, LastName);
		}

	}
}

