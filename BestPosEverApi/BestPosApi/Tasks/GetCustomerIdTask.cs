using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simpler;
using WebApplication1.Models;

namespace WebApplication1.Tasks
{
	public class GetCustomerIdTask : OutSimpleTask<string>
	{
		public override void Execute()
		{
			var sql = "SELECT AutoCustPrefix as Prefix, AutoCustSuffix as Suffix, AutoCustSize as Length from DBA.PosControls";
			Out = SharedDb.PosimDb.Get<SuffixPrefix>(sql).StringValue;
			sql =
				string.Format("update PosControls set  AutoCustSuffix =  AutoCustSuffix + 1");
			SharedDb.PosimDb.Execute(sql);
		}
	}
}