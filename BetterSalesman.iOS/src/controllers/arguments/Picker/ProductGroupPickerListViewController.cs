using System;
using MonoTouch.UIKit;
using BetterSalesman.Core.BusinessLayer;

namespace BetterSalesman.iOS
{
	partial class ProductGroupPickerListViewController : UITableViewController
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
            
            NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(
                UIImage.FromBundle(ic_back),
                UIBarButtonItemStyle.Plain, 
                (o, s) => NavigationController.PopViewControllerAnimated(true))
            ,false);
        }
			
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			ProductGroupsDataSource.ReloadProductGroups();

			SubscribeToProductGroupPickedEvent();
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);

			UnsubscribeFromProductGroupPickedEvent();
		}
        
        #endregion

		private void ProductGroupPicked(ProductGroup newSelectedProductGroup)
		{
			InvokeOnMainThread(() => NavigationController.PopViewControllerAnimated(true));
		}

		private void SubscribeToProductGroupPickedEvent()
		{
			if (productGroupsDataSource != null && !isSubscribedToSelectedProductGroupChangedEvent)
			{
				productGroupsDataSource.ProductGroupPicked += ProductGroupPicked;
				isSubscribedToSelectedProductGroupChangedEvent = true;
			}
		}

		private void UnsubscribeFromProductGroupPickedEvent()
		{
			if (productGroupsDataSource != null && isSubscribedToSelectedProductGroupChangedEvent)
			{
				productGroupsDataSource.ProductGroupPicked -= ProductGroupPicked;
				isSubscribedToSelectedProductGroupChangedEvent = false;
			}
		}
	}
}
