using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.PointOfService;
using WebApplication1.Models;
using WebApplication1.Printing;
using WebApplication1.Tasks;

namespace WebApplication1.Controllers
{
    public class PrintInvoiceController : ApiController
    {


        // GET: api/PrintInvoice/5
        public bool Get(string id)
        {
	        try
	        {
		        var invoice = new LoadInvoiceTask
		        {
					Id = id,
					Status = InvoiceStatus.Posted
		        }.ExecuteMe().Invoice;
		        new ReceiptPrinter().printReceipt(invoice);
		        return true;
	        }
	        catch (Exception ex)
	        {
		        Console.WriteLine(ex);
	        }
	        return false;

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
