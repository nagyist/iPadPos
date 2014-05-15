using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CouponsController : ApiController
    {
		// GET: api/Coupons
		public IEnumerable<Item> Get()
		{
			var selectString = string.Format(ItemsController.select + " Where ItemId in ({0})", "'CPNMILITITARY','CPNFSG','CPNFSR','CPNBKCG','CPNBKCR','CPNBOWCLUB','CPNBOWR','CPNTEXT'");
			return SharedDb.GetMany<Item>(selectString);
		}

        // GET: api/Coupons/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Coupons
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Coupons/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Coupons/5
        public void Delete(int id)
        {
        }
    }
}
