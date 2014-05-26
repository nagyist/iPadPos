using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.PointOfService;
using WebApplication1.Models;

namespace WebApplication1.Printing
{
    internal static class Receipt40Col
    {
        public enum Alignment
        {
            Left,
            Right,
            Center
        };

        private const int charLimit = 45;
        private const int column1Width = 10;
        private const int column2Width = 21;
        private const int column3Width = 8;

        public static string GetStandardReceipt(Invoice invoice)
        {
            var sb = new StringBuilder();
	        try
	        {
		        getStandardHeader(sb);
		        SetContent(invoice, sb);
		        var customer = invoice.Customer;
		        SetInvoiceFooter(customer, sb);
		        AddPointBalance(customer, sb);
		        AddDisclaimer(sb);
	        }
	        catch (Exception ex)
	        {
		        Console.WriteLine(ex);
	        }
	        return sb.ToString();
        }

	    public static string GetInvoiceHeaderBitmap()
	    {
		    try
		    {
			    var image = SharedDb.PosimDb.Get<PrintImage>(
				    "select RecordId,ImageName, Image as ImageData from DBA.PrintableImages where RecordId in(select HeaderImageRecordID from InvoiceSettings where name = 'Default Receipt')");
			    var path = Path.Combine(Path.GetTempPath(), string.Format("tempImage-{0}.bmp", image.RecordId));
			    TypeConverter tc = TypeDescriptor.GetConverter(typeof (Bitmap));
			    Bitmap bitmap1 = (Bitmap) tc.ConvertFrom(image.ImageData);
			    if (File.Exists(path))
				    File.Delete(path);
			    bitmap1.Save(path);
			    return path;
		    }
		    catch (Exception ex)
		    {
				Console.WriteLine(ex);
			    return null;
		    }
	    }

	    public static string GetInvoiceFooterBitmap()
	    {
			try{
				var image = SharedDb.PosimDb.Get<PrintImage>(
					"select RecordId,ImageName, Image as ImageData  from DBA.PrintableImages where RecordId in(select FooterImageRecordID from InvoiceSettings where name = 'Default Receipt')");
				var path = Path.Combine(Path.GetTempPath(), string.Format("tempImage-{0}.bmp", image.RecordId));
				TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
				Bitmap bitmap1 = (Bitmap)tc.ConvertFrom(image.ImageData);
				if (File.Exists(path))
					File.Delete(path);
				bitmap1.Save(path);
				return path;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
	    }

		//public static string GetStandardReceipt(Buyback buyback)
		//{
		//	var sb = new StringBuilder();
		//	getStandardHeader(sb);
		//	SetContent(buyback,sb);
		//	var customer = buyback.Customer;
		//	CustomerDatabase.RefreshCustomer(ref customer);
		//	SetInvoiceFooter(customer,sb);
		//	AddDisclaimer(sb);
		//	return sb.ToString();
		//}

        public static void getStandardHeader(StringBuilder sb)
        {

			var h = SharedDb.PosimDb.GetString("select header from InvoiceSettings where name = 'Default Receipt'"); 
			using (var reader = new StringReader(h))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					sb.AppendLine(padLine(line.Trim()));
				}
			}
			sb.AppendLine(padLine(DateTime.Now.ToLongDateString()));
	        sb.AppendLine();
	        //sb.AppendLine();
	        //sb.AppendLine(Bold(padLine("Hut no. 8 - Sacramento")));
	        //sb.AppendLine(padLine("1338 Howe Ave. #100"));
	        //sb.AppendLine(padLine("(916) 538-9175"));
	        //sb.AppendLine(padLine("www.hutno8.com"));
	        //sb.AppendLine(padLine(DateTime.Now.ToLongDateString()));
	        //sb.AppendLine();
        }

        public static string Bold(string input)
        {
            return Escape("bC") + input + Escape("N");
        }

        public static string BoldAndBig(string input, int size)
        {
            return Escape("4C") + Escape("bC") + input + Escape("N");
        }

        static string Escape(string input)
        {
            return ((char)27).ToString() + "|" + input;
        }
        public static void SetInvoiceFooter(Customer customer ,StringBuilder sb)
        {
			var h = SharedDb.PosimDb.GetString("select footer from InvoiceSettings where name = 'Default Receipt'");
			using (var reader = new StringReader(h))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					sb.AppendLine(padLine(line.Trim()));
				}
			}
			if (customer.OnAccount != 0)
				sb.AppendLine(padLine(string.Format("You current Account Balance is: {0}", customer.OnAccount.ToString("C2"))));
			sb.AppendLine();

        }

        public static void AddPointBalance(Customer customer, StringBuilder sb)
        {
			//sb.AppendLine(padLine("Your new point balance is:", Alignment.Center));
			//sb.AppendLine();
			//sb.AppendLine(BoldAndBig(string.Format("{0}/30", customer.LoyaltyPoints).PadLeft(13), 4));
			//sb.AppendLine();
			//sb.AppendLine(padLine("When you reach 30 points you get 20%"));
			//sb.AppendLine(padLine("off an entire purchase."));
			//sb.AppendLine();
        }
        public static void AddDisclaimer(StringBuilder sb)
        {
			//sb.AppendLine();
			//sb.AppendLine(padLine("Items with an attached Hut no. 8 tag and"));
			//sb.AppendLine(padLine("recept may be exchanged within 7 days."));
			//sb.AppendLine(padLine("Clearance items, New Items,"));
			//sb.AppendLine(padLine("and Jewelry are Final Sale"));
        }

		//public static void SetContent(Buyback buyback, StringBuilder sb)
		//{
		//	sb.AppendLine("Customer:");
		//	sb.AppendLine(buyback.Customer.ToString());
		//	if(!string.IsNullOrEmpty(buyback.Customer.Email))
		//		sb.AppendLine(buyback.Customer.Email);
		//	sb.AppendLine();

		//	//foreach (var item in invoice.BuybackItems)
		//	//{
		//	//    sb.AppendLine(padLine(item.ItemId,item.Description,item.BuyAmount.ToString("C2")));
		//	//}
		//	sb.AppendLine(padLine(string.Format("Today you sold us {0} items.", buyback.ItemsQty)));
		//	sb.AppendLine();
		//	if (buyback.OnAccountAmount > 0)
		//	{
		//		sb.AppendLine(
		//			Bold(
		//			padLine(string.Format("{0} has been applied to your account.",
		//								  buyback.OnAccountAmount.ToString("C2")))));
		//	}
		//	else
		//		sb.AppendLine(
		//			Bold(padLine(string.Format("For a Cash total of: {0}", buyback.PostedAmount.Value.ToString("C2")))));
		//	sb.AppendLine();
		//	sb.AppendLine();
		//}
        public static void SetContent(Invoice invoice, StringBuilder sb)
        {
            sb.AppendLine("Customer:");
            sb.AppendLine(invoice.Customer.ToString());
            if (!string.IsNullOrEmpty(invoice.Customer.Email))
                sb.AppendLine(invoice.Customer.Email);
            sb.AppendLine();
            sb.AppendLine();

            foreach (var item in invoice.Lines)
            {
                sb.AppendLine(padLine(item.ItemId,item.Description,item.Price.ToString("C2")));
                if (item.Discount != 0)
                    sb.AppendLine(padLine("", "*Discounted","-" + item.Discount.ToString("C2")));
            }
            sb.AppendLine();
            sb.AppendLine(padLine(string.Format("Sub Total: {0}", invoice.SubTotal.ToString("C2")),Alignment.Right));
            if(invoice.TotalDiscount != 0)
                sb.AppendLine(padLine(string.Format("Discount: {0}", invoice.TotalDiscount.ToString("C2")), Alignment.Right));
           // sb.AppendLine(padLine(string.Format("Tax: {0}", invoice.TotalTax.ToString("C2")), Alignment.Right));
            sb.AppendLine(Bold(padLine(string.Format("Total: {0}", (invoice.Total).ToString("C2")), Alignment.Right)));
            sb.AppendLine();
            sb.AppendLine("Tender:");
            foreach (var payment in invoice.Payments.Where(x=> x.Amount != 0))
            {
                sb.AppendLine(string.Format("{0}: {1}", payment.PaymentType.Description, payment.Amount.ToString("C2")));
                var change = Math.Abs(payment.Change);
                if (change != 0)
                {
                    //sb.AppendLine(string.Format("Received: {0}", (payment.Amount).ToString("C2")));
                    sb.AppendLine(string.Format("Change: {0}", change.ToString("C2")));
                }
            }
            sb.AppendLine();
            sb.AppendLine();
        }


        private static string formatLine(string line, Alignment alignment = Alignment.Center)
        {
            string newLine = "";
            if (line.Length > charLimit)
            {
            }
            else
            {
                if (alignment == Alignment.Left)
                {
                    newLine = line;
                }
                else
                {
                    newLine = padLine(line, alignment);
                }
            }

            return newLine;
        }

        private static string padLine(string line, Alignment alignment = Alignment.Center)
        {
            string newLine = "";
            int curentSpace = 0;
            int spacesNeeded = 0;
            if (alignment == Alignment.Right)
            {
                spacesNeeded = charLimit - line.Length;
            }
            else if (alignment == Alignment.Center)
            {
                spacesNeeded = (charLimit - line.Length)/2;
            }
            while (curentSpace <= spacesNeeded)
            {
                newLine += " ";
                curentSpace += 1;
            }
            newLine += line;
            return newLine;
        }

        private static string padLine(string col, string col2, string col3)
        {
            string newline = "";
            if (col.Length > column1Width)
                newline = col.Substring(0, column1Width).PadRight(column1Width + 5);
            else
                newline = col.PadRight(column1Width + 5);

            if (col2.Length > column2Width)
                newline += col2.Substring(0, column2Width).PadRight(column2Width + 2);
            else
                newline += col2.PadRight(column2Width + 2);

            newline += col3.PadLeft(column3Width);

            return newline;
        }

        public static string extraLines(int lineCount)
        {
            string extraLines = "";
            int curInt = 0;
            while (curInt < lineCount)
            {
                extraLines += "\r\n";
                curInt += 1;
            }
            return extraLines;
        }
    }
}