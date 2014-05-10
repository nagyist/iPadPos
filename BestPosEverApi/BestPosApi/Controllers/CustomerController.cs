using System;
using System.Web.Http;
using WebApplication1.Helpers;
using WebApplication1.Models;
using WebApplication1.Tasks;

namespace WebApplication1.Controllers
{
	public class CustomerController : ApiController
	{
		public const string select = @"select
RecordID as RecordID,
	CustomerID,
	BillCompany as Company,
	BillFName as FirstName,
	BillMI as MiddleInitial,
	BillLName as LastName,
	BillAddress as Address1, 
	BillAddress2 as Address2, 
	BillCity as City, 
	BillState as State, 
	BillZip as Zip, 
	BillCountry as Country, 
	BillPhone as HomePhone, 
	BillExt as PhoneExt, 
	Misc1 as CellPhone,
	Misc2, 
	LastUpdate, 
	DateCreated,
	Email,
  (if(select SUM(Balance) from
      dba.ARItems where(ParentRecordID = C.RecordID) and
      (DocType in(0,1))) is null then 0
    else(select SUM(Balance) from
        dba.ARItems where(ParentRecordID = C.RecordID) and
        (DocType in(0,1)))
    endif) *-1 as OnAccount
from DBA.Customers as C ";


		// GET: api/Customer/5
		public Customer Get(string id)
		{
			string query = select + string.Format("where customerid = '{0}'", id);
			return SharedDb.Get<Customer>(query);
		}

		// POST: api/Customer
		public Customer Post([FromBody] Customer value)
		{
			var custIdTask = new GetCustomerIdTask();
			custIdTask.Execute();

			value.CustomerID = custIdTask.Out;

			string date = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss");
			string ins =
				string.Format(@"INSERT INTO DBA.Customers (CustomerID, BillCompany, BillFName, BillMI, BillLName, BillAddress, BillAddress2, BillCity, BillState, BillZip, BillCountry, BillPhone, BillExt, Misc1,Misc2, LastUpdate, DateCreated, EMail, ShipFName) 
								VALUES  ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18})",
					value.CustomerID.GetSqlCompatible(true),
					value.Company.GetSqlCompatible(true),
					value.FirstName.GetSqlCompatible(true),
					value.MiddleInitial.GetSqlCompatible(true),
					value.LastName.GetSqlCompatible(true),
					value.Address1.GetSqlCompatible(true),
					value.Address2.GetSqlCompatible(true),
					value.City.GetSqlCompatible(true),
					value.State.GetSqlCompatible(true),
					value.Zip.GetSqlCompatible(true),
					value.Country.GetSqlCompatible(true),
					value.HomePhone.GetSqlCompatible(true),
					value.PhoneExt.GetSqlCompatible(true),
					value.CellPhone.GetSqlCompatible(true),
					value.Misc2.GetSqlCompatible(true),
					date.GetSqlCompatible(true),
					date.GetSqlCompatible(true),
					value.Email.GetSqlCompatible(),
					value.FirstName.GetSqlCompatible()
					);
			SharedDb.Execute(ins);
			return Get(value.CustomerID);
		}

		// PUT: api/Customer/5
		public bool Put([FromBody] Customer value)
		{
			string date = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss");
			var update = string.Format(@"Update Customers set
			BillCompany = {1},
			BillFName = {2},
			BillMI = {3},
			BillLName = {4}, 
			BillAddress = {5}, 
			BillAddress2 = {6}, 
			BillCity = {7}, 
			BillState = {8}, 
			BillZip = {9}, 
			BillCountry = {10}, 
			BillPhone = {11},
			BillExt = {12}, 
			Misc1 = {13},
			Misc2 = {14}, 
			LastUpdate = {15}, 
			EMail = {16}, 
			ShipFName = {2}
			where CustomerID = {0}", value.CustomerID.GetSqlCompatible(true),
					value.Company.GetSqlCompatible(true),
					value.FirstName.GetSqlCompatible(true),
					value.MiddleInitial.GetSqlCompatible(true),
					value.LastName.GetSqlCompatible(true),
					value.Address1.GetSqlCompatible(true),
					value.Address2.GetSqlCompatible(true),
					value.City.GetSqlCompatible(true),
					value.State.GetSqlCompatible(true),
					value.Zip.GetSqlCompatible(true),
					value.Country.GetSqlCompatible(true),
					value.HomePhone.GetSqlCompatible(true),
					value.PhoneExt.GetSqlCompatible(true),
					value.CellPhone.GetSqlCompatible(true),
					value.Misc2.GetSqlCompatible(true),
					date.GetSqlCompatible(true),
					value.Email.GetSqlCompatible());

			return SharedDb.Execute(update) > 0;
		}
	}
}