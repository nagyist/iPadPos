using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class NewProductsController : ApiController
    {
		// GET: api/NewNewProducts
		public IEnumerable<Item> Get()
		{
			var selectString = string.Format(ItemsController.select + " Where ItemId in ({0})", "'NPCC0016','NPCC0014','NPHHSW','NPWA415','NPGTMIP','NPHH260','NPHHSLHB','NPHHBRAC','NPDT199'");
			var items = SharedDb.PosimDb.GetMany<Item>(selectString);
			return items;
		}


        // GET: api/NewNewProducts/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/NewNewProducts
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/NewNewProducts/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/NewNewProducts/5
        public void Delete(int id)
        {
        }
    }
}
