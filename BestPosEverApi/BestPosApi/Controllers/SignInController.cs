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
    public class SignInController : ApiController
    {
        // GET: api/SignIn
        public IEnumerable<Signature> Get()
        {
	        return SharedDb.SqlServer.GetMany<Signature>("select * from signature");
	        // return new string[] { "value1", "value2" };
        }

        // GET: api/SignIn/5
        public bool Get(string id)
        {
	        var sql = string.Format("SELECT * FROM SalesPeople where SalesPersonId = {0}", id.GetSqlCompatible());
			var success = SharedDb.PosimDb.GetMany<SalesPerson>(sql).Any();

			return success;
        }

        // POST: api/SignIn
        public bool Post([FromBody]string value)
        {
			var success = SharedDb.PosimDb.Get<SalesPerson>(string.Format("SELECT * FROM SalesPeople where SalesPersonId = {0}",value.GetSqlCompatible())) != null;

	        return success;

        }

        // PUT: api/SignIn/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SignIn/5
        public void Delete(int id)
        {
        }
    }
}
