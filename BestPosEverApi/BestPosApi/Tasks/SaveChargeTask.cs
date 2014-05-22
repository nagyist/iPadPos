using System;
using Simpler;
using WebApplication1.Models;

namespace WebApplication1.Tasks
{
	public class SaveChargeTask : SimpleTask
	{
		public ChargeDetails Charge { get; set; }

		public override void Execute()
		{
			try
			{
				SharedDb.SqlServer.Execute(@"INSERT INTO [dbo].[ChargeDetails]
           ([Token]
           ,[Amount]
           ,[ReferenceId]
           ,[IsRefunded]
           ,[AmountRefunded]
           ,[Created]
           ,[InvoiceId])
     VALUES
           (@Token,
           @Amount,
           @ReferenceId,
           @IsRefunded,
           @AmountRefunded,
           @Created,
           @InvoiceId)", Charge);


				Charge.Signature.Token = Charge.Token;
				SharedDb.SqlServer.Execute(@"INSERT INTO [dbo].[Signature]
           ([Token]
           ,[Data])
     VALUES
           (@Token
           ,@Data)", Charge.Signature);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
	}
}