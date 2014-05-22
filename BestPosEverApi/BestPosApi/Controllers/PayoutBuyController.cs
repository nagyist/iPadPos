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
    public class PayoutBuyController : ApiController
    {
		 public const string SelectQuery = @" select 
							RecordID,
							InvoiceID as Id,
							CustomerID,
							InvDate as InvoiceDate,
							BillLName + ', ' + BillFName  as CustomerName,
							RegisterID,
							InvTotal as Total,
							(select case when OnAcctAmt <> 0 then 1 else 0 end as IsOnAccount from BYR_BUYHDRS b where b.InvoiceID = i.InvoiceID ) as IsOnAccount
						From WInvHeaders i

			";
        // GET: api/PayoutBuy
		public IEnumerable<BuyInvoice> Get()
		{
			var query = SelectQuery + " where InvoiceID like 'BUY%' Order by BillLName, BillFName ";
			return SharedDb.PosimDb.GetMany<BuyInvoice>(query);
        }

        // GET: api/PayoutBuy/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PayoutBuy
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/PayoutBuy/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PayoutBuy/5
        public void Delete(int id)
        {
        }
    }
}
