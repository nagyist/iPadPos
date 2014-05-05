using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ItemsController : ApiController
    {
	    const string select = @"Select
RecordId,
ItemID,
Description,
OurRetail as Price,
Misc1,
Misc2,
Misc3,
Misc4,
Misc5,
Available as Qty,
TaxCode,
TransCode,
AvgCost as Cost
from Items 
";
        // GET: api/Items
		//public IEnumerable<Item> Get(string search)
		//{
		//	var searchQuery = select + string.Format("where itemid = '{0}'" , search);
		//	return SharedDb.GetMany<Item>(searchQuery);
		//}

        // GET: api/Items/5
        public Item Get(string id)
		{
			var searchQuery = select + string.Format("where itemid = '{0}'", id);
			return SharedDb.Get<Item>(searchQuery);
        }

        // POST: api/Items
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Items/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Items/5
        public void Delete(int id)
        {
        }
    }
}
