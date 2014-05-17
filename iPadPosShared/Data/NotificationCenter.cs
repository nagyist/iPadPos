using System;

namespace iPadPos
{
	public class NotificationCenter
	{
		static NotificationCenter shared;
		public static NotificationCenter Shared {
			get {
				return shared ?? (shared = new NotificationCenter());
			}
		}
		public NotificationCenter ()
		{
		}

		public event Action CouponsChanged;
		public void ProcCouponsChanged()
		{
			if (CouponsChanged != null)
				CouponsChanged ();
		}

		public event Action NewProductChanged;
		public void ProcNewProductChanged()
		{
			if (NewProductChanged != null)
				NewProductChanged ();
		}
	}
}

