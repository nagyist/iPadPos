using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using WebGrease.Css.Extensions;

namespace WebApplication1.Controllers
{
    public class CouponsController : ApiController
    {
		// GET: api/Coupons
		public IEnumerable<Coupon> Get()
		{
			var selectString = string.Format(ItemsController.select + " Where ItemId in ({0})", "'CPNMILITITARY','CPNFSG','CPNFSR','CPNBKCG','CPNBKCR','CPNBOWCLUB','CPNBOWR','CPNTEXT'");
			var coupons = SharedDb.PosimDb.GetMany<Coupon>(selectString).ToList();
			coupons.Add(new Coupon
			{
				Description = "Memorial Day Sale",
				ItemID = "memsale2014",
				DiscountPercent = .3f,
				TransCode = "S",
				StartDate = DateTime.Today,
				EndDate = new DateTime(2014,5,20),
				SelectedItemsOnly = true,
			});
			coupons.ForEach(x =>
			{
				float percent;
				if(float.TryParse(x.Misc1, out percent))
					x.DiscountPercent = percent;
			});
			return coupons;
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
