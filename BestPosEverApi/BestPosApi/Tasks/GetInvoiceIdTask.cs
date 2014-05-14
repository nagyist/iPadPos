using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simpler;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Tasks
{
	public class GetInvoiceIdTask : OutSimpleTask<string>
	{
		public InvoiceStatus InvoiceStatus { get; set; }
		public string RegisterId { get; set; }
		public override SimpleTask Execute()
		{
			bool exists = true;
			while (exists)
			{
				var sql =
					string.Format(
						"select {0}InvPrefix as Prefix, {0}InvSuffix as Suffix, {0}InvSize as Length from PosimRegisters where RegisterId = '{1}'",
						InvoiceStatus == InvoiceStatus.Posted ? "P" : "W", RegisterId);
				Out = SharedDb.Get<SuffixPrefix>(sql).StringValue;
				sql =
					string.Format("update PosimRegisters set  {0}InvSuffix =  {0}InvSuffix + 1 where RegisterId = '{1}'",
						InvoiceStatus == InvoiceStatus.Posted ? "P" : "W", RegisterId);
				SharedDb.Execute(sql);
				string existsString = string.Format("select count(InvoiceId) from {0}InvHeaders where InvoiceID = {1}",
					InvoiceStatus == InvoiceStatus.Posted ? "P" : "W", Out.GetSqlCompatible());
				exists = SharedDb.GetInt(existsString) > 0;
			}
			return this;
		}
	}
}