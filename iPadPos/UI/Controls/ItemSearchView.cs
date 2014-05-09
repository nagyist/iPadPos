using System;
using MonoTouch.UIKit;
using System.Threading.Tasks;

namespace iPadPos
{
	public class ItemSearchView : UISearchBar
	{
		public event Action<Item> ItemSelected;
		UISearchDisplayController displayController;

		public ItemSearchView ()
		{
			var i = 1;
			this.SearchButtonClicked += (sender, e) => Search ();
		}

		async Task Search()
		{
			this.UserInteractionEnabled = false;
			try{
				var item = await WebService.Main.GetItem (this.Text);
				if(item == null || ItemSelected == null)
					return;
				ItemSelected(item);
				this.Text = "";
				//this.EndEditing(true);
				//this.ResignFirstResponder();

			}
			catch(Exception ex) {
				Console.WriteLine (ex);
				this.BecomeFirstResponder ();
			}
			finally{
				UserInteractionEnabled = true;
			}
		}

		UIViewController parent;
		public UIViewController Parent {
			get {
				return parent;
			}
			set {
				parent = value;
				displayController = new UISearchDisplayController (this, parent);
			}
		}
	}
}

