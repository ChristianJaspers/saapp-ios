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
				UnsubscribeFromSelectedProductGroupChangedEvent();

				this.productGroupsDataSource = value;
				this.TableView.Source = value;

				SubscribeToSelectedProductGroupChangedEvent();
			}
		}

		public ProductGroupPickerListViewController(IntPtr handle) : base(handle)
		{
			this.isSubscribedToSelectedProductGroupChangedEvent = false;
		}
			
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			SubscribeToSelectedProductGroupChangedEvent();
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);

			UnsubscribeFromSelectedProductGroupChangedEvent();
		}

		private void SelectedProductGroupChanged(ProductGroup newSelectedProductGroup)
		{
			InvokeOnMainThread(() =>
			{
				NavigationController.PopViewControllerAnimated(true);
			});
		}

		private void SubscribeToSelectedProductGroupChangedEvent()
		{
			if (this.productGroupsDataSource != null && !isSubscribedToSelectedProductGroupChangedEvent)
			{
				this.productGroupsDataSource.SelectedProductGroupChanged += SelectedProductGroupChanged;
				isSubscribedToSelectedProductGroupChangedEvent = true;
			}
		}

		private void UnsubscribeFromSelectedProductGroupChangedEvent()
		{
			if (this.productGroupsDataSource != null && isSubscribedToSelectedProductGroupChangedEvent)
			{
				this.productGroupsDataSource.SelectedProductGroupChanged -= SelectedProductGroupChanged;
				isSubscribedToSelectedProductGroupChangedEvent = false;
			}
		}
	}
}
