using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using WebApplication1.Printing;

namespace WebApplication1.Controllers
{
    public class PrintInvoiceController : ApiController
    {
        // GET: api/PrintInvoice
        public IEnumerable<string> Get()
        {
			new ReceiptPrinter().printReceipt(new Invoice()
			{
				Customer = new Customer(),
			});

            return new string[] { "value1", "value2" };
        }

        // GET: api/PrintInvoice/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PrintInvoice
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/PrintInvoice/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PrintInvoice/5
        public void Delete(int id)
        {
        }
    }
}
