using System;
using System.Globalization;

namespace iPadPos
{
	public class Payment : BaseModel
	{
		public int Id { get; set; }
		public PaymentType PaymentType { get; set; }
		double amount;
		public double Amount {
			get {
				return amount;
			}
			set {
				if (this.ProcPropertyChanged (ref amount, value))
					ProcPropertyChanged ("AmountString");
			}
		}
		public string AmountString
		{
			get{ return Amount.ToString ("C"); }
			set{ Amount = double.Parse (value, NumberStyles.Currency); }
		}
	}
}

