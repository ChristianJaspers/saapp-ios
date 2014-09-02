using System;
using MonoTouch.UIKit;
using BetterSalesman.Core.BusinessLayer;

namespace BetterSalesman.iOS
{
    partial class ProductGroupPickerListViewController : BaseUITableViewController
	{
        const string ic_back = "ic_back";
        
		private bool isSubscribedToSelectedProductGroupChangedEvent;
		private ProductGroupsDataSource productGroupsDataSource;

		public ProductGroupsDataSource ProductGroupsDataSource
		{
			get
			{
                return productGroupsDataSource;
			}

			set
			{
				UnsubscribeFromProductGroupPickedEvent();

				productGroupsDataSource = value;
				TableView.Source = value;

				SubscribeToProductGroupPickedEvent();
			}
		}

		public ProductGroupPickerListViewController(IntPtr handle) : base(handle)
		{
			isSubscribedToSelectedProductGroupChangedEvent = false;
		}
        
        #region Lifecycle
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            Title = I18n.ProductGroups;
            
            LeftBarButtonAsArrowIconOnly();
        }
			
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

//			ProductGroupsDataSource.ReloadProductGroups();

			SubscribeToProductGroupPickedEvent();
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);

			UnsubscribeFromProductGroupPickedEvent();
		}

		public void RefreshProductGroupsList()
		{
			InvokeOnMainThread(() =>
			{
				TableView.ReloadData();
			});
		}
        
        #endregion

		private void ProductGroupPicked(ProductGroup newSelectedProductGroup)
		{
			InvokeOnMainThread(() => NavigationController.PopViewControllerAnimated(true));
		}

		private void SubscribeToProductGroupPickedEvent()
		{
			if (this.productGroupsDataSource != null && !this.isSubscribedToSelectedProductGroupChangedEvent)
			{
				this.productGroupsDataSource.ProductGroupPicked += ProductGroupPicked;
				this.productGroupsDataSource.ProductGroupsReloaded += RefreshProductGroupsList;
				this.isSubscribedToSelectedProductGroupChangedEvent = true;
			}
		}

		private void UnsubscribeFromProductGroupPickedEvent()
		{
			if (this.productGroupsDataSource != null && this.isSubscribedToSelectedProductGroupChangedEvent)
			{
				this.productGroupsDataSource.ProductGroupPicked -= ProductGroupPicked;
				this.productGroupsDataSource.ProductGroupsReloaded -= RefreshProductGroupsList;
				this.isSubscribedToSelectedProductGroupChangedEvent = false;
			}
		}
	}
}
