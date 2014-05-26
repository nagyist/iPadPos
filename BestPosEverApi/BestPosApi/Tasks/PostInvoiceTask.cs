using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Simpler;
using WebApplication1.Helpers;
using WebApplication1.Models;
using WebApplication1.Printing;

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
				var winvTask = new SaveWorkingInvoiceTask
				{
					Date = Date,
					In = In,
				};
				winvTask.Execute();
				var wId = In.Id;
				string invDate = Date.ToString("yyyy'-'MM'-'dd HH':'mm':'ss");
				var invTask = new GetInvoiceIdTask
				{
					RegisterId = In.RegisterId,
					InvoiceStatus = InvoiceStatus.Posted,
				};
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
						VALUES (0,{0},'2300/jan/01 00:00',{1},0,{2},'Net30',{3},0,0,{3},0,0,{4},0)", In.Customer.RecordID,
							invDate.GetSqlCompatible(), pId.GetSqlCompatible(), acctPayment.Amount, In.RegisterId.GetSqlCompatible());
					SharedDb.PosimDb.Execute(insertArQuery);

					SharedDb.PosimDb.Execute(string.Format("call ApplyToOldestAux({0})", In.Customer.RecordID));
				}

				//Create posted invoice
				string copyQuery = string.Format("CALL CopyWInv2PInv ('{0}','{1}', '{2}' )", wId, pId, invDate);
				SharedDb.PosimDb.Execute(copyQuery);

				//Save sharge and signature
				if (In.ChargeDetail != null)
				{
					In.ChargeDetail.InvoiceId = pId;
					new SaveChargeTask{Charge = In.ChargeDetail}.Execute();
					
				}
			
			//
				//Post
				string precQuery = "select RecordID from PInvHeaders where InvoiceID = " + pId.GetSqlCompatible(true);
				var recordId = SharedDb.PosimDb.GetInt(precQuery);

				string postQuery = string.Format("CALL PostPInvLines ({0},{1},{2},0,{3},'')", recordId, pId.GetSqlCompatible(true),
					In.Customer.CustomerID.GetSqlCompatible(true),
					invDate.GetSqlCompatible(true));
				SharedDb.PosimDb.Execute(postQuery);

				if(SendEmailTask.IsValidEmail(In.Customer.Email))
					Task.Factory.StartNew(() => new SendEmailTask
					{
						Invoice = In,
					}.Execute());
				else
				{
					Task.Factory.StartNew(() => new ReceiptPrinter().printReceipt(In));
				}

				//Cleanup
				SharedDb.PosimDb.Execute(string.Format("CALL DisposeWInv ({0})", wId.GetSqlCompatible(true)));

				//t.Complete();
				Out = true;
			}
		}
	}
}