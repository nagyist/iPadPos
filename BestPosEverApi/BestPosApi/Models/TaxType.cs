using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class TaxType
	{
		public string Id { get; set; }
		public string Description { get; set; }
		public double Rate { get; set; }
	}
}