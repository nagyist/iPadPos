using System;
using Newtonsoft.Json;
using SQLite;

namespace iPadPos
{
	public class Customer : BaseModel, iDirty
	{
		public Customer()
		{
			State = "AK";
		}

		public int RecordId { get; set; }
		public bool IsDirty {get;set;}
		[PrimaryKey]
		public string CustomerId { get; set; }

		string company;
		public string Company {
			get {
				return company;
			}
			set { ProcPropertyChanged (ref company, value); }
		}

		string firstName;
		public string FirstName {
			get {
				return firstName;
			}
			set { ProcPropertyChanged (ref firstName, value); }
		}

		string lastName;
		public string LastName {
			get {
				return lastName;
			}
			set { ProcPropertyChanged (ref lastName, value); }
		}

		string middleName;
		public string MiddleName {
			get {
				return middleName;
			}
			set { ProcPropertyChanged (ref middleName, value); }
		}

		string address;
		[JsonProperty("Address1")]
		public string Address {
			get {
				return address;
			}
			set { ProcPropertyChanged (ref address, value); }
		}

		string address2;
		public string Address2 {
			get {
				return address2;
			}
			set { ProcPropertyChanged (ref address2, value); }
		}

		string city;
		public string City {
			get {
				return city;
			}
			set { ProcPropertyChanged (ref city, value); }
		}

		string state;
		public string State {
			get {
				return state;
			}
			set { ProcPropertyChanged (ref state, value); }
		}

		string zip;
		public string Zip {
			get {
				return zip;
			}
			set { ProcPropertyChanged (ref zip, value); }
		}

		string country;
		public string Country {
			get {
				return country;
			}
			set { ProcPropertyChanged (ref country, value); }
		}

		string homePhone;
		public string HomePhone {
			get {
				return homePhone;
			}
			set { ProcPropertyChanged (ref homePhone, value); }
		}

		string phoneExt;
		public string PhoneExt {
			get {
				return phoneExt;
			}
			set { ProcPropertyChanged (ref phoneExt, value); }
		}

		string cellPhone;
		public string CellPhone {
			get {
				return cellPhone;
			}
			set { ProcPropertyChanged (ref cellPhone, value); }
		}

		string misc2;
		public string Misc2 {
			get {
				return misc2;
			}
			set { ProcPropertyChanged (ref misc2, value); }
		}

		DateTime lastUpdate;
		public DateTime LastUpdate {
			get {
				return lastUpdate;
			}
			set { ProcPropertyChanged (ref lastUpdate, value); }
		}

		public DateTime DateCreated { get; set; }

		string email;
		public string Email {
			get {
				return email;
			}
			set { ProcPropertyChanged (ref email, value); }
		}

		double onAccount;
		public double OnAccount {
			get {
				return onAccount;
			}
			set { ProcPropertyChanged (ref onAccount, value); }
		}

		public bool IsNew {get;set;}

		public override string ToString ()
		{
			if (CustomerId == Settings.Shared.CashCustomer)
				return "Cash Customer";
			return string.Format ("{0} {1}",FirstName.UppercaseFirst(), LastName.UppercaseFirst());
		}

	}
}

