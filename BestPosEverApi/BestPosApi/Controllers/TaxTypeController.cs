using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using WebApplication1.Tasks;

namespace WebApplication1.Controllers
{
    public class TaxTypeController : ApiController
    {
        // GET: api/TaxType
        public IEnumerable<TaxType> Get()
        {
	        var task = new GetTaxTypeTask();
			task.Execute();
	        return task.Out;
        }

        // GET: api/TaxType/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TaxType
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/TaxType/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TaxType/5
        public void Delete(int id)
        {
        }
    }
}
