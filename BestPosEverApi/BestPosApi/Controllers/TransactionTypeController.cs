using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class TransactionTypeController : ApiController
    {
        // GET: api/TransactionType
        public IEnumerable<TransactionType> Get()
        {
	        return new[]
	        {
				new TransactionType
				{
					Description = "Sales",
					Id = "S",
					Multiplier = 1,
				}, new TransactionType
				{
					Description = "Returns",
					Id = "R",
					Multiplier = -1,
				}, 
	        };
        }

        // GET: api/TransactionType/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TransactionType
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/TransactionType/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TransactionType/5
        public void Delete(int id)
        {
        }
    }
}
