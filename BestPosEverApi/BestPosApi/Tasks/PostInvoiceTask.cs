using System;
using System.Linq;
using System.Transactions;
using Simpler;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Tasks
{
	public class PostInvoiceTask : InOutSimpleTask<Invoice, bool>
	{
		DateTime Date = DateTime.Now;

		public override void Execute()
		{
			//using (var t = new TransactionScope())
			{
				//Get working invoice Id
				var invTask = new GetInvoiceIdTask
				{
					RegisterId = In.RegisterId,
					InvoiceStatus = InvoiceStatus.Working,
				};
				invTask.Execute();
				string invDate = Date.ToString("yyyy'-'MM'-'dd HH':'mm':'ss");
				string date = Date.Date.ToString("yyyy'-'MM'-'dd HH':'mm':'ss");
				string wId = In.Id = invTask.Out;
				//Create Working Invoice
				string rexordQuery = "select max(recordID) + 1 from DBA.WInvHeaders";
				int recordId = SharedDb.GetInt(rexordQuery);
				//var recordId = int.Parse(recordIdstring);
				string winHdrArray = recordId + ",'" + In.Id + "','" +
				                     In.Customer.CustomerID +
				                     "','" + invDate + "','" +
				                     Date.ToString("yyyy'-'MM'-'dd") + "','" +
									 In.Customer.Company.GetSqlCompatible(false) + "','" + In.Customer.FirstName.GetSqlCompatible(false) +
									 "','" + In.Customer.MiddleInitial.GetSqlCompatible(false) + "','" +
									 In.Customer.LastName.GetSqlCompatible(false) + "','" + In.Customer.Address1.GetSqlCompatible(false) +
									 "','" + In.Customer.Address2.GetSqlCompatible(false) + "','" +
									 In.Customer.City.GetSqlCompatible(false) + "','" + In.Customer.State.GetSqlCompatible(false) + "','" +
									 In.Customer.Zip.GetSqlCompatible(false) + "','" + In.Customer.Country.GetSqlCompatible(false) + "','" +
									 In.Customer.HomePhone + "','B','" + In.Customer.Email.GetSqlCompatible(false) + "','" +
									 In.Customer.ShipName.GetSqlCompatible(false) + "'," + In.Total;
				string winHdrInsertString =
					@"INSERT INTO DBA.WInvHeaders
							(RecordID,
							InvoiceID,
							CustomerID,
							InvDate,
							DateOnly,
							BillCompany,
							BillFName,
							BillMI, 
							BillLName,
							BillAddress,
							BillAddress2,
							BillCity,
							BillState,
							BillZip,
							BillCountry,
							BillPhone,
							RegisterID,
							Email,
							ShipFName,
							InvTotal) 
						VALUES (" + winHdrArray + ")";
				bool success = SharedDb.Execute(winHdrInsertString) > 0;


				invTask.InvoiceStatus = InvoiceStatus.Posted;
				invTask.Execute();
				string pId = invTask.Out;

				//Update Rewards
				var acctPayment = In.Payments.FirstOrDefault(x => x.PaymentType.Id == "Acct" && x.Amount != 0);
				if (acctPayment != null)
				{
					//insert AR Items
					string insertArQuery =
						string.Format(
							@"INSERT INTO DBA.ARItems(LockFlag,PARENTRECORDID,CLOSEDON,DOCDATE, DOCTYPE,REFNO,TERMS,SALE,PAYMENT,APPLIED,BALANCE,BALANCEFORWARDED,PERIOD,REGISTERID,LATEFEECODE)
						VALUES (0,{0},'2300/jan/01 00:00',{1},0,{2},'Net30',{3},0,0,{3},0,0,{4},0)", In.Customer.RecordID, date.GetSqlCompatible(), pId.GetSqlCompatible(), acctPayment.Amount, In.RegisterId.GetSqlCompatible());
					SharedDb.Execute(insertArQuery);

					SharedDb.Execute(string.Format("call ApplyToOldestAux({0})", In.Customer.RecordID));
				}
				//Insert WInvLines
				In.GetCompinedLines().ForEach(x =>
				{
					string winLineInsertString = string.Format(
						"insert into DBA.WInvLines(LockFlag,ParentRecordID,LineOrder,TransCode,Qty,ItemID,Description,PriceLevel,Price,TaxCode,Cost,OnHand,FinalPrice,SerialNum,IsItemGrpDisc,Points,SerialNumComment)" +
						"VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}) ",
						0, recordId, x.LineOrder, x.TransCode.GetSqlCompatible(true),
						x.Qty,x.ItemId.GetSqlCompatible(true), x.Description.GetSqlCompatible(true),x.PriceLevel.GetSqlCompatible(true), x.Price.ToString("F2"), x.TaxCode.GetSqlCompatible(true), x.Cost,
						x.OnHand,
						x.FinalPrice.ToString("F2"),x.SerialNumber.GetSqlCompatible(true),"''",x.Points,"''");
					SharedDb.Execute(winLineInsertString);
				});


				//Create posted invoice
				string copyQuery = string.Format("CALL CopyWInv2PInv ('{0}','{1}', '{2}' )", wId, pId, invDate);
				SharedDb.Execute(copyQuery);

				
				//Post
				string precQuery = "select RecordID from PInvHeaders where InvoiceID = " + pId.GetSqlCompatible(true);
				recordId = SharedDb.GetInt(precQuery);

				string postQuery = string.Format("CALL PostPInvLines ({0},{1},{2},0,{3},'')", recordId, pId.GetSqlCompatible(true),
					In.Customer.CustomerID.GetSqlCompatible(true),
					invDate.GetSqlCompatible(true));
				SharedDb.Execute(postQuery);

			

				//Cleanup
				SharedDb.Execute(string.Format("CALL DisposeWInv ({0})", wId.GetSqlCompatible(true)));

				//t.Complete();
				Out = true;
			}
		}
	}
}