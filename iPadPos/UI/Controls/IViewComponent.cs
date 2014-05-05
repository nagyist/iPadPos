using System;

namespace iPadPos
{
	public interface IViewComponent
	{
		int Column { get; set; }

		int ColumnSpan { get; set; }

		int RowSpan { get; set; }

		int Row { get; set; }
	}
}

