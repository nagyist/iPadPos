using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Drawing;


namespace iPadPos
{
	public class QuickItemsViewController : UICollectionViewController
	{
		public UIColor ItemBackgroundColor  = UIColor.Clear;
		public Action<Item> AddItem { get; set; }
		public Func<Task<List<Item>>> GetItems { get; set; }
		public QuickItemsViewController () : base(new UICollectionViewFlowLayout{
			ScrollDirection = UICollectionViewScrollDirection.Horizontal,
			ItemSize = new SizeF(250,80),
			SectionInset = new UIEdgeInsets (5,20,5,20),
		} )
		{
			this.EdgesForExtendedLayout = UIRectEdge.None;
			this.View.BackgroundColor = UIColor.Clear;
			this.CollectionView.BackgroundColor = UIColor.Clear;
			CollectionView.RegisterClassForCell (typeof(ItemCollectionViewCell), ItemCollectionViewCell.Key);
		}
		public List<Item> Items = new List<Item> ();
		public override UICollectionViewCell GetCell (UICollectionView collectionView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var itemCell = (ItemCollectionViewCell)collectionView.DequeueReusableCell (ItemCollectionViewCell.Key, indexPath);
			//itemCell.Frame = new System.Drawing.RectangleF (0, 0, 200, 100);
			var item = Items [indexPath.Row];
			itemCell.BackgroundColor = ItemBackgroundColor;
			itemCell.Item = item;

			return itemCell;
		}

		public override int GetItemsCount (UICollectionView collectionView, int section)
		{
			return Items.Count;
		}
		public override void ItemSelected (UICollectionView collectionView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			if (AddItem != null)
				AddItem (Items [indexPath.Row]);
			CollectionView.DeselectItem (indexPath, true);
		}
		public override async void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			if (GetItems == null)
				return;
			Items = await GetItems ();
			this.CollectionView.ReloadData ();
		}
	}
}

