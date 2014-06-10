using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PaymentTypeController : ApiController
    {
        // GET: api/PaymentTypeS
		internal static PaymentType[] PaymentTypes = new []{new PaymentType
				{
					Id = "Cash",
					Description = "Cash",
					IsActive = true,
					SortOrder = 0,
				},
	           new PaymentType
	           {
		           Id = "Visa",
				   Description = "Credit Card",
				   IsActive = true,
				   LaunchAppUrl = "square-up://",
				   SortOrder = 1,
	           },
			   new PaymentType
			   {
				 Id = "Acct",
  				 Description = "On Account",
				 IsActive = true,
				 SortOrder = 2

			   },
			   new PaymentType
			   {
				 Id  = "Check: ",
				 Description = "Check",
				 IsActive = true,
				 HasExtra = true,
				 SortOrder = 3,
			   },

            };
        public IEnumerable<PaymentType> Get()
        {
			return PaymentTypes;
        }

		//// GET: api/PaymentType/5
		//public string Get(int id)
		//{
		//	return "value";
		//}

		//// POST: api/PaymentType
		//public void Post([FromBody]string value)
		//{
		//}

		//// PUT: api/PaymentType/5
		//public void Put(int id, [FromBody]string value)
		//{
		//}

		//// DELETE: api/PaymentType/5
		//public void Delete(int id)
		//{
		//}
    }
}
