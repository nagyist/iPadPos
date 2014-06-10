using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class SettingsController : ApiController
    {
        // GET: api/Settings
        public IEnumerable<Setting> Get()
        {
	        return SharedDb.SqlServer.GetMany<Setting>("select * from Settings");
        }

        // GET: api/Settings/5
        public string Get(string id)
        {
            return "value";
        }

        // POST: api/Settings
		public void Post([FromBody]Setting value)
        {

        }

        // PUT: api/Settings/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Settings/5
        public void Delete(int id)
        {
        }
    }
}
