using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simpler;
using WebApplication1.Models;

namespace WebApplication1.Tasks
{
	public class GetTaxTypeTask : OutSimpleTask<List<TaxType>>
	{
		public override void Execute()
		{
			var taxTypes = SharedDb.PosimDb.GetMany<TaxType>(@"
					select 1 as Id, TaxRate1/100 as Rate, TaxDesc1 as Description from InvControls 
					union

					select 2 as Id, TaxRate2/100 as Rate, TaxDesc2 as Description from InvControls 
					").ToList();
			var combined = taxTypes.Sum(x => x.Rate);
			taxTypes.Add(new TaxType{Rate = combined, Description = "Combined", Id = "3"});

			Out = taxTypes;
		}
	}
}