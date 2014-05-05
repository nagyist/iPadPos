namespace WebApplication1.Models
{
	public class Item
	{
		#region public properties

		public int RecordId { get; set; }

		public string ItemID { get; set; }

		public string Misc1 { get; set; }

		public string Misc2 { get; set; }

		public string Misc3 { get; set; }

		public string Misc4 { get; set; }

		public string Misc5 { get; set; }

		public int TaxCode { get; set; }

		public string Description { get; set; }

		public int Qty { get; set; }

		public double Price { get; set; }

		public string TransCode { get; set; }

		public double Cost { get; set; }

		#endregion
	}
}