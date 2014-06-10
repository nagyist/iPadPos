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
    public class InvoiceController : ApiController
    {
 
        // GET: api/Invoice/5
        public Invoice Get(string id)
        {
	        var task = new LoadInvoiceTask
	        {
				Status = InvoiceStatus.Posted,
		        Id = id,
	        }.ExecuteMe();
	        return task.Invoice;
        }

        // POST: api/Invoice
        public PostedInvoiceResult Post([FromBody]Invoice value)
        {
	        var task = new PostInvoiceTask
	        {
		        In = value,
	        };
			task.Execute();
	        return new PostedInvoiceResult
	        {
		        Success = task.Out,
		        InvoiceId = task.PostedInvoiceId,

	        };
        }

        // PUT: api/Invoice/5
        public void Put(int id, [FromBody]Invoice value)
        {
        }

        // DELETE: api/Invoice/5
        public void Delete(int id)
        {
        }
    }
}
