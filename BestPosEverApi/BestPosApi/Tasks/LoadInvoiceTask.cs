using Simpler;
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
							SalesPerson as SalesPersonId,
							InvTotal as Total
						From 
			";

		public string Id { get; set; }
		public Invoice Invoice { get; private set; }

		public override void Execute()
		{
			string query = string.Format("{0} {1}  where InvoiceId = {2}", SelectQuery, Status == InvoiceStatus.Posted ? "pinvheaders" : "Winvheaders", Id.GetSqlCompatible());
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

from {0}InvLines where ParentRecordId = {1} order by LineOrder"
				,Status == InvoiceStatus.Posted ? "p" : "w",Invoice.RecordId.ToString().GetSqlCompatible());
			InvoiceLine[] lines = SharedDb.PosimDb.GetMany<InvoiceLine>(linesQuery);
			lines.ForEach(x => x.CalculateDiscount());
			//TODO remove payments
			Invoice.ParseLines(lines);
		}
	}
}