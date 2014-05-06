using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class SuffixPrefix
	{
		public string Prefix { get; set; }

		public int Suffix { get; set; }

		public int Length { get; set; }

		public string StringValue
		{
			get
			{
				return Prefix + Suffix.ToString().PadLeft(Length - Prefix.Length,'0');
			}

		}
	}
}