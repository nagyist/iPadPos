﻿using Simpler;
using WebApplication1.Models;
using WebApplication1.Helpers;
using WebGrease.Css.Extensions;
using WebApplication1.Controllers;
namespace WebApplication1.Tasks
{
	public class LoadInvoiceTask : SimpleTask
	{
		public InvoiceStatus Status { get; set; }
		public const string SelectQuery = @" select 
							RecordID,
							InvoiceID as Id,
							CustomerID,
							InvDate as InvoiceDate,
							BillLName + ', ' + BillFName  as CustomerName,
							RegisterID,
							InvTotal as Total
						From WInvHeaders
			";

		public string Id { get; set; }
		public Invoice Invoice { get; private set; }

		public override void Execute()
		{
			string query = string.Format("{0}  where InvoiceId = {1}", SelectQuery, Id.GetSqlCompatible());
			Invoice = SharedDb.PosimDb.Get<Invoice>(query);

			if (!string.IsNullOrEmpty(Invoice.CustomerId))
				Invoice.Customer =
					SharedDb.PosimDb.Get<Customer>(string.Format("{0} where customerid = {1}", CustomerController.select,
						Invoice.CustomerId.GetSqlCompatible()));

			string linesQuery = string.Format(@"select 
RecordId,
Cost,
Description,
FinalPrice,
ItemId,
LineOrder,
OnHand,
ParentRecordId,
Points,
Price,
PriceLevel,
Qty,
SerialNum as SerialNumber,
TaxCode,
--Discount,
Transcode

from WInvLines where ParentRecordId = {0} order by LineOrder",
				Invoice.RecordId.ToString().GetSqlCompatible());
			InvoiceLine[] lines = SharedDb.PosimDb.GetMany<InvoiceLine>(linesQuery);
			lines.ForEach(x => x.CalculateDiscount());
			//TODO remove payments
			Invoice.ParseLines(lines);
		}
	}
}