using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simpler;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Tasks
{
	public class SaveWorkingInvoiceTask : InOutSimpleTask<Invoice, bool>
	{
		public DateTime Date = DateTime.Now;
		public string InvoiceId { get; set; }
		public override void Execute()
		{
			//using (var t = new TransactionScope())
			{
				//Get working invoice Id

				if(In.RecordId > 0)
					Update();
				else
					Insert();
			
				//Insert WInvLines
				In.GetCompinedLines().ForEach(line =>
				{
					var winLineInsertString = string.Format(
						"insert into DBA.WInvLines(LockFlag,ParentRecordID,LineOrder,TransCode,Qty,ItemID,Description,PriceLevel,Price,TaxCode,Cost,OnHand,FinalPrice,SerialNum,IsItemGrpDisc,Points,SerialNumComment)" +
						"VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}) ",
						0, line.ParentRecordId, line.LineOrder, line.TransCode.GetSqlCompatible(true),
						line.Qty, line.ItemId.GetSqlCompatible(true), line.Description.GetSqlCompatible(true), line.PriceLevel.GetSqlCompatible(true), line.Price.ToString("F2"), line.TaxCode.ToString().GetSqlCompatible(true), line.Cost,
						line.OnHand,
						line.FinalPrice.ToString("F2"), line.SerialNumber.GetSqlCompatible(true), "''", line.Points, "''");
					SharedDb.PosimDb.Execute(winLineInsertString);
				});

				Out = true;
			}
		}

		public void Insert()
		{
			//Get working invoice Id
			var invTask = new GetInvoiceIdTask
			{
				RegisterId = In.RegisterId,
				InvoiceStatus = InvoiceStatus.Working,
			};
			invTask.Execute();
			string invDate = Date.ToString("yyyy'-'MM'-'dd HH':'mm':'ss");
			InvoiceId = In.Id = invTask.Out;
			//Create Working Invoice
			var recordQuery = "select max(recordID) + 1 from DBA.WInvHeaders";
			int recordId = SharedDb.PosimDb.GetInt(recordQuery);
			In.RecordId = recordId;
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
								 In.Customer.HomePhone + "',"+ In.RegisterId.GetSqlCompatible() + ",'" + In.Customer.Email.GetSqlCompatible(false) + "','" +
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
			bool success = SharedDb.PosimDb.Execute(winHdrInsertString) > 0;
		}

		public void Update()
		{
			string updateQuery = string.Format(
				@"UPDATE WInvHeaders SET
							InvoiceID = {0},
							CustomerID = {1},
							InvDate = {2},
							DateOnly = {3},
							BillCompany = {4},
							BillFName = {5},
							BillMI = {6}, 
							BillLName = {7},
							BillAddress = {8},
							BillAddress2 = {9},
							BillCity = {10},
							BillState = {11},
							BillZip = {12},
							BillCountry = {13},
							BillPhone = {14},
							RegisterID = {15},
							Email = {16},
							ShipFName = {17},
							InvTotal = {18}

							where RecordId = {19} ", In.Id.GetSqlCompatible(), In.CustomerId.GetSqlCompatible(),
															Date.ToString("yyyy'-'MM'-'dd HH':'mm':'ss").GetSqlCompatible(),
															Date.ToString("yyyy'-'MM'-'dd").GetSqlCompatible(),
															In.Customer.Company.GetSqlCompatible(),
															In.Customer.FirstName.GetSqlCompatible(),
															In.Customer.MiddleInitial.GetSqlCompatible(),
															In.Customer.LastName.GetSqlCompatible(),
															In.Customer.Address1.GetSqlCompatible(),
															In.Customer.Address2.GetSqlCompatible(),
															In.Customer.City.GetSqlCompatible(),
															In.Customer.State.GetSqlCompatible(),
															In.Customer.Zip.GetSqlCompatible(),
															In.Customer.Country.GetSqlCompatible(),
															In.Customer.HomePhone.GetSqlCompatible(),
															In.RegisterId.GetSqlCompatible(),
															In.Customer.Email.GetSqlCompatible(),
															In.Customer.FirstName.GetSqlCompatible(),
															In.Total,
															In.RecordId);
			SharedDb.PosimDb.Execute(updateQuery);
			//Clear lines
			var deleteLinesQuery = string.Format("delete from WInvLines where ParentRecordID = {0}", In.RecordId);
			SharedDb.PosimDb.Execute(deleteLinesQuery);
		}

		
	}
}