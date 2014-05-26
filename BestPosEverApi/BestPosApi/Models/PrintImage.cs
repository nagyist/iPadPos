using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class PrintImage
	{
		public int RecordId { get; set; }
		public string ImageName { get; set; }
		public byte[] ImageData { get; set; }
	}
}