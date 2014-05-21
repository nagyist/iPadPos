using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;


namespace iPadPos
{
	public class QuickItemsViewController : UICollectionViewController
	{
		public UIColor ItemBackgroundColor  = UIColor.Clear;
		public UIColor AlternateItemBackgroundColor  = UIColor.Clear;
		public Action<Item> AddItem { get; set; }
		public int Sections = 1;
		public Func<int,Task<List<Item>>> GetItems { get; set; }
		public QuickItemsViewController () : base(new UICollectionViewFlowLayout{
			ScrollDirection = UICollectionViewScrollDirection.Horizontal,
			ItemSize = new SizeF(110 ,80),
			SectionInset = new UIEdgeInsets (5,20,5,20),
		} )
		{
			this.EdgesForExtendedLayout = UIRectEdge.None;
			this.View.BackgroundColor = UIColor.Clear;
			this.CollectionView.BackgroundColor = UIColor.Clear;
			CollectionView.RegisterClassForCell (typeof(ItemCollectionViewCell), ItemCollectionViewCell.Key);
		}
		public List<Item>[] Items = new List<Item>[0];
		public override UICollectionViewCell GetCell (UICollectionView collectionView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var itemCell = (ItemCollectionViewCell)collectionView.DequeueReusableCell (ItemCollectionViewCell.Key, indexPath);
			//itemCell.Frame = new System.Drawing.RectangleF (0, 0, 200, 100);
			var item = Items[indexPath.Section][indexPath.Row];
			itemCell.BackgroundColor = item.UseAlterate() ? AlternateItemBackgroundColor : ItemBackgroundColor;
			itemCell.Item = item;

			return itemCell;
		}
		public override int NumberOfSections (UICollectionView collectionView)
		{
			return Sections;
		}
		public override int GetItemsCount (UICollectionView collectionView, int section)
		{
			if (Items [section] == null)
				return 0;
			return Items[section].Count;
		}
		public override void ItemSelected (UICollectionView collectionView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			if (AddItem != null)
				AddItem (Items[indexPath.Section][indexPath.Row]);
			CollectionView.DeselectItem (indexPath, true);
		}
		public override async void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			await ReloadData ();
		}
		public async Task ReloadData()
		{
			if (GetItems == null)
				return;
			try{
				Items = new List<Item>[Sections];
				foreach(var x in  Enumerable.Range(0,Sections))
				{
					Items[x] = await GetItems (x);
				};
				this.CollectionView.ReloadData ();
			}
			catch(Exception ex) {
				Console.WriteLine (ex);
			}
		}
	}
}

