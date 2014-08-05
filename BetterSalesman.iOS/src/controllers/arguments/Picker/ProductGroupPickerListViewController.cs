using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;
using System.Diagnostics;
using BetterSalesman.Core.BusinessLayer;
using System.Collections.Generic;

namespace BetterSalesman.iOS
{
	partial class ProductGroupPickerListViewController : UITableViewController
	{
		private bool isSubscribedToSelectedProductGroupChangedEvent;
		private ProductGroupsDataSource productGroupsDataSource;

		public ProductGroupsDataSource ProductGroupsDataSource
		{
			get
			{
				return this.productGroupsDataSource;
			}

			set
			{
				UnsubscribeFromProductGroupPickedEvent();

				this.productGroupsDataSource = value;
				this.TableView.Source = value;

				SubscribeToProductGroupPickedEvent();
			}
		}

		public ProductGroupPickerListViewController(IntPtr handle) : base(handle)
		{
			this.isSubscribedToSelectedProductGroupChangedEvent = false;
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

		private void ProductGroupPicked(ProductGroup newSelectedProductGroup)
		{
			InvokeOnMainThread(() =>
			{
				NavigationController.PopViewControllerAnimated(true);
			});
		}

		private void SubscribeToProductGroupPickedEvent()
		{
			if (this.productGroupsDataSource != null && !isSubscribedToSelectedProductGroupChangedEvent)
			{
				this.productGroupsDataSource.ProductGroupPicked += ProductGroupPicked;
				isSubscribedToSelectedProductGroupChangedEvent = true;
			}
		}

		private void UnsubscribeFromProductGroupPickedEvent()
		{
			if (this.productGroupsDataSource != null && isSubscribedToSelectedProductGroupChangedEvent)
			{
				this.productGroupsDataSource.ProductGroupPicked -= ProductGroupPicked;
				isSubscribedToSelectedProductGroupChangedEvent = false;
			}
		}
	}
}
