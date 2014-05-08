using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Controllers;

namespace WebApplication1.Models
{
    public class CustomerSearchController : ApiController
    {
		// GET: api/Customer/5
		public IEnumerable<Customer> Get(string id)
		{
			string custSearchQuery = CustomerController.select + string.Format(" WHERE (CustomerID = '{0}') OR (BillPhone like'%{0}%') OR (Misc1 like'%{0}%') or (BillLname like '{0}%')", id);
			return SharedDb.GetMany<Customer>(custSearchQuery);
		}
    }
}
