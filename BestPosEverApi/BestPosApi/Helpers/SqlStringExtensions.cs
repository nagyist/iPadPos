using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Helpers
{
	public static class SqlStringExtensions
	{
		public static string GetSqlCompatible(this string stringIn, bool quote = true)
		{
			string newString = "";
			if (!string.IsNullOrEmpty(stringIn))
			{
				newString = stringIn.Replace("'", "''");
			}
			return quote ? string.Format("'{0}'",newString) : newString;
		}
	}
}