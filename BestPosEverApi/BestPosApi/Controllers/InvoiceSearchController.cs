using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class InvoiceSearchController : ApiController
    {
        // GET: api/InvoiceSearch
	    public const string SelectQuery = @" select 
							RecordID,
							InvoiceID as Id,
							CustomerID,
							InvDate as InvoiceDate,
							BillLName + ', ' + BillFName  as CustomerName,
							RegisterID,
							InvTotal as Total
						From WInvHeaders

			";
        public IEnumerable<Invoice> Get()
		{
			string invDate = DateTime.Today.AddDays(-1).ToString("yyyy'-'MM'-'dd");
			var query = SelectQuery + " where InvoiceID not like 'BUY%' and InvDate > " + invDate.GetSqlCompatible();
	       return SharedDb.GetMany<Invoice>(query);
        }

        // GET: api/InvoiceSearch/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/InvoiceSearch
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/InvoiceSearch/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/InvoiceSearch/5
        public void Delete(int id)
        {
        }
    }
}
