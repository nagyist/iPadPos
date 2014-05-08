using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class PaymentType
	{
		public string Id { get; set; }
		public string Description { get; set; }
		public bool IsActive { get; set; }
		public bool HasExtra { get; set; }
		public string LaunchAppUrl { get; set; }
		public int SortOrder { get; set; }
	}
}