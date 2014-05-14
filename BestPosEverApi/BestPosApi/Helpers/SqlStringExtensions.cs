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
		public static string UppercaseFirst(this string s)
		{
			// Check for empty string.
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			// Return char and concat substring.
			return char.ToUpper(s[0]) + s.Substring(1).ToLower();
		}
	}
}