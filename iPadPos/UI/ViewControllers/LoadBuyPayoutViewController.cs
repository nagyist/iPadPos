using System;

namespace iPadPos
{
	public class LoadBuyPayoutViewController : LoadInvoiceViewController
	{
		public LoadBuyPayoutViewController ()
		{
			Title = "Pay out buy";
			PreferredContentSize = new System.Drawing.SizeF (500, 480);
			TableView.CellIdentifier = BuyInvoiceCell.Key;
			TableView.CreateCellFunc = () => new BuyInvoiceCell ();
			TableView.BindCellAction = (cell, obj) => (cell as BuyInvoiceCell).Invoice = obj as BuyInvoice;
		}

		protected override async void ReloadData ()
		{

			TableView.DataSource = await WebService.Main.GetUnpostedBuys();
		}
	}
}

