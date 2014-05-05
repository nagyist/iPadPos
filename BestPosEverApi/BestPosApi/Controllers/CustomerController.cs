using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CustomerController : ApiController
    {
	    const string select = @"select
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
	Email
from DBA.Customers ";
		const string insert = "INSERT INTO DBA.Customers (CustomerID, BillCompany, BillFName, BillMI, BillLName, BillAddress, BillAddress2, BillCity, BillState, BillZip, BillCountry, BillPhone, BillExt, Misc1,Misc2, LastUpdate, DateCreated, EMail, ShipFName) VALUES  ()";
        // GET: api/Customer
		public IEnumerable<Customer> Get(string search)
		{
			string custSearchQuery = select + string.Format(" WHERE (CustomerID = '{0}') OR (BillPhone like'%{0}%') OR (Misc1 like'%{0}%') or (BillLname like '{0}%')",search);
			return SharedDb.GetMany<Customer>(custSearchQuery);
		}

        // GET: api/Customer/5
        public Customer Get(int id)
        {
	        return SharedDb.Get<Customer>(select + string.Format("where customerid = '{0}'",id));
        }

        // POST: api/Customer
        public void Post([FromBody]Customer value)
        {
        }

        // PUT: api/Customer/5
        public void Put(int id, [FromBody]Customer value)
        {
        }

        // DELETE: api/Customer/5
        public void Delete(int id)
        {
        }
    }
}
