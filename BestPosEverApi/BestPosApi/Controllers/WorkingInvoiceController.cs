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
    public class WorkingInvoiceController : ApiController
    {

        // GET: api/WorkingInvoice/5
		public Invoice Get(string id)
		{
			var task = new LoadInvoiceTask
			{
				Status = InvoiceStatus.Posted,
				Id = id,
			}.ExecuteMe();
			return task.Invoice;
		}

        // POST: api/WorkingInvoice
        public string Post([FromBody]Invoice value)
        {
	        var winvTask = new SaveWorkingInvoiceTask
	        {
				In = value,

	        };
	        winvTask.Execute();
	        return winvTask.InvoiceId;
        }

        // PUT: api/WorkingInvoice/5
		public void Put(int id, [FromBody]Invoice value)
        {
        }

        // DELETE: api/WorkingInvoice/5
        public bool Delete(string id)
		{
			var deleteLinesQuery = string.Format("delete from WInvLines where ParentRecordID = {0}", id);
			SharedDb.PosimDb.Execute(deleteLinesQuery);
	        var deleteQuery = string.Format("delete from WInvHeaders where RecordID = {0}", id);
	        SharedDb.PosimDb.Execute(deleteQuery);
	        return true;
		}
    }
}
