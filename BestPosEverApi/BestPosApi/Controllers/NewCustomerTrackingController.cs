using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class NewCustomerTrackingController : ApiController
    {
        // GET: api/NewCustomerTracking
        public IEnumerable<Item> Get()
        {
	        var selectString = string.Format(ItemsController.select + " Where ItemId in ({0})", "'NCWALK','NCCLIST','NCFACE','NCMAC','NCWEB','NCWOM','NCOTHER'");
	        return SharedDb.GetMany<Item>(selectString);
        }

        // GET: api/NewCustomerTracking/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/NewCustomerTracking
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/NewCustomerTracking/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/NewCustomerTracking/5
        public void Delete(int id)
        {
        }
    }
}
