﻿using System;

namespace iPadPos
{
	public class InvoiceTableView : ObservableTableView
	{
		public InvoiceTableView () : base(MonoTouch.UIKit.UITableViewStyle.Plain)
		{
			CellIdentifier = InvoiceLineCell.Key;
			SectionHeaderHeight =RowHeight = 60;
			ContentOffset = new System.Drawing.PointF (0, -100);
			BackgroundColor = Theme.Current.BackgroundGray;
		}

		Invoice invoice;
		public Invoice Invoice {
			get {
				return invoice;
			}
			set {
				invoice = value ?? new Invoice();
				DataSource = invoice.Items;
			}
		}
		protected override void BindCell (MonoTouch.UIKit.UITableViewCell cell, object item, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			(cell as InvoiceLineCell).Line = item as InvoiceLine;
		}
		protected override MonoTouch.UIKit.UITableViewCell CreateCell (string reuseId)
		{
			return new InvoiceLineCell ();
		}
		protected override MonoTouch.UIKit.UIView CreateHeader ()
		{
			return this.DequeueReusableCell (InvoiceHeaderCell.Key) as InvoiceHeaderCell ?? new InvoiceHeaderCell ();
		}
	}
}

