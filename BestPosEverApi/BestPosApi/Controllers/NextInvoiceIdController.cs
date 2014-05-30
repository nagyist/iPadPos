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
    public class NextInvoiceIdController : ApiController
    {
      
        // GET: api/NextInvoiceId/5
        public string Get(int id)
        {
			var invTask = new GetInvoiceIdTask
			{
				RegisterId = id.ToString(),
				InvoiceStatus = InvoiceStatus.Posted,
			};
			invTask.Execute();
			return invTask.Out;
        }

        // POST: api/NextInvoiceId
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/NextInvoiceId/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/NextInvoiceId/5
        public void Delete(int id)
        {
        }
    }
}
