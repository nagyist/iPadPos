using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using Microsoft.PointOfService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Org.BouncyCastle.Asn1.Cms;
using WebApplication1.Models;

namespace WebApplication1.Printing
{
	class ReceiptPrinter
	{
		PosExplorer explorer;
		PosPrinter m_Printer;

		public void printReceipt(Invoice invoice, bool openDrawer = false)
		{

			// return;
			setupPrinter();
			try
			{
				m_Printer.Open();

				//Get the exclusive control right for the opened device.
				//Then the device is disable from other application.
				m_Printer.Claim(1000);


				//Enable the device.
				bool result = m_Printer.DeviceEnabled = true;

				// this call also causes the "It is not initialized" error
				//string health = m_Printer.CheckHealth(HealthCheckLevel.External);
			}
			catch (PosControlException)
			{
				//  ChangeButtonStatus();
			}
			string receiptString = Receipt40Col.GetStandardReceipt(invoice);
			string barcodeString = invoice.Id ?? "";
			while (barcodeString.Length < 8)
			{
				barcodeString = " " + barcodeString;
			}
			// int test = m_Printer.JrnLineChars


			//var file = Path.GetFullPath("Resources/logo.bmp");
			// m_Printer.PrintNormal(PrinterStation.Receipt,((char)27).ToString() + "|cA" + receiptString);
			//m_Printer.PrintBitmap(PrinterStation.Receipt, file, 300, PosPrinter.PrinterBitmapCenter);

			var headerImage = Receipt40Col.GetInvoiceHeaderBitmap();
			if (headerImage != null)
			{
				m_Printer.PrintBitmap(PrinterStation.Receipt, headerImage, 400, PosPrinter.PrinterBitmapCenter);
			}
			//m_Printer.PrintMemoryBitmap(new BitmapData());
			m_Printer.PrintNormal(PrinterStation.Receipt, receiptString);

			var footerImage = Receipt40Col.GetInvoiceFooterBitmap();
			if (footerImage != null)
				m_Printer.PrintBitmap(PrinterStation.Receipt, footerImage, 400, PosPrinter.PrinterBitmapCenter);
			m_Printer.PrintNormal(PrinterStation.Receipt, Receipt40Col.extraLines(8));
			if (openDrawer)
				m_Printer.PrintNormal(PrinterStation.Receipt, ((char) 27).ToString() + "|\x07");



		}



		//public void printReceipt(InvoiceLine[] giftCards)
		//{
		//	try
		//	{
		//		// return;
		//		setupPrinter();
		//		try
		//		{
		//			m_Printer.Open();

		//			//Get the exclusive control right for the opened device.
		//			//Then the device is disable from other application.
		//			m_Printer.Claim(1000);


		//			//Enable the device.
		//			bool result = m_Printer.DeviceEnabled = true;

		//			// this call also causes the "It is not initialized" error
		//			//string health = m_Printer.CheckHealth(HealthCheckLevel.External);
		//		}
		//		catch (PosControlException)
		//		{
		//			//  ChangeButtonStatus();
		//		}
		//		foreach (var invoiceLine in giftCards)
		//		{

		//			string receiptString = Receipt40Col.GetStandardReceipt(invoiceLine);
		//			// int test = m_Printer.JrnLineChars


		//			var file = Path.GetFullPath("Resources/logo.bmp");
		//			// m_Printer.PrintNormal(PrinterStation.Receipt,((char)27).ToString() + "|cA" + receiptString);
		//			m_Printer.PrintBitmap(PrinterStation.Receipt, file, 300, PosPrinter.PrinterBitmapCenter);
		//			m_Printer.PrintNormal(PrinterStation.Receipt, receiptString);
		//			m_Printer.PrintNormal(PrinterStation.Receipt, Receipt40Col.extraLines(8));
		//			Thread.Sleep(3000);
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		ErrorLogging.logError(ex);

		//	}

		//}

		//public void printReceipt(Buyback buyback, bool openDrawer = false)
		//{
		//	//return;
		//	setupPrinter();
		//	try
		//	{
		//		m_Printer.Open();

		//		//Get the exclusive control right for the opened device.
		//		//Then the device is disable from other application.
		//		m_Printer.Claim(1000);


		//		//Enable the device.
		//		bool result = m_Printer.DeviceEnabled = true;

		//		// this call also causes the "It is not initialized" error
		//		//string health = m_Printer.CheckHealth(HealthCheckLevel.External);

		//		string receiptString = Receipt40Col.GetStandardReceipt(buyback);
		//		string barcodeString = buyback.Id.ToString();
		//		while (barcodeString.Length < 8)
		//		{
		//			barcodeString = " " + barcodeString;
		//		}
		//		// int test = m_Printer.JrnLineChars

		//		var file = Path.GetFullPath("Resources/logo.bmp");

		//		// m_Printer.PrintNormal(PrinterStation.Receipt,((char)27).ToString() + "|cA" + receiptString);
		//		m_Printer.PrintBitmap(PrinterStation.Receipt, file, 300, PosPrinter.PrinterBitmapCenter);
		//		m_Printer.PrintNormal(PrinterStation.Receipt, receiptString);

		//		m_Printer.PrintNormal(PrinterStation.Receipt, Receipt40Col.extraLines(8));
		//		if (openDrawer)
		//			m_Printer.PrintNormal(PrinterStation.Receipt, ((char)27).ToString() + "|\x07");
		//	}
		//	catch (Exception ex)
		//	{
		//		ErrorLogging.logError(ex);
		//	}
		//}

		void setupPrinter()
		{

			//Create PosExplorer
			var posExplorer = new PosExplorer();
			var devices = posExplorer.GetDevices("PosPrinter");
			DeviceInfo deviceInfo = null;

			try
			{
				var printerName = ConfigurationManager.AppSettings["ReceiptPrinter"];

				deviceInfo = devices.OfType<DeviceInfo>().FirstOrDefault(x => x.ServiceObjectName.Contains(printerName)) ??
				             devices.OfType<DeviceInfo>().FirstOrDefault(x => x.ServiceObjectName.Contains("Star"));
				// this call returns a valid object
				var p = posExplorer.CreateInstance(deviceInfo);

				m_Printer = (PosPrinter) p; // posExplorer.CreateInstance(deviceInfo);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
				//   ChangeButtonStatus();
				return;
			}
		}
	}



}
