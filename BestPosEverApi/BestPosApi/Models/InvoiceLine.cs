using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class InvoiceLine
	{
		public InvoiceLine()
		{
			Description = "";
			ItemId = "";
			PriceLevel = "";
			SerialNumber = "";
			TaxCode = "";
			TransCode = "";
		}

		public double Cost { get; set; }

		public string Description { get; set; }
		
		public double FinalPrice { get; set; }

		public string ItemId { get; set; }

		public int LineOrder { get; set; }

		public int OnHand { get; set; }
		
		public int ParentRecordId { get; set; }

		public double Points { get; set; }

		public double Price { get; set; }

		public string PriceLevel { get; set; }

		public int Qty { get; set; }

		public int RecordId { get; set; }

		public string SerialNumber { get; set; }

		public string TaxCode { get; set; }

		public double Discount { get; set; }

		public string TransCode { get; set; }

		public void CalculateDiscount()
		{
			if (Qty == 0 || TransCode == "O")
				return;
			var price = Price*Qty;
			Discount = (FinalPrice - price)/Qty;
		}
	}
}