using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BetterSalesman.Core.BusinessLayer;

namespace BetterSalesman.iOS
{
	partial class ProductGroupPickerButtonViewController : UIViewController
	{
		private const string SegueIdProductGroupPickerEmbeded = "segueIdProductGroupPickerSubview";
		private const string SegueIdProductGroupPickerList = "segueIdProductGroupPickerList";

		private bool isSubscribedToSelectedProductGroupChangedEvent;

		private ProductGroupsDataSource productsGroupDataSource;

		public ProductGroupsDataSource ProductGroupsDataSource 
		{ 
			get
			{
				return this.productsGroupDataSource;
			}

			set
			{
				UnsubscribeFromSelectedProductGroupChangedEvent();

				this.productsGroupDataSource = value;
				SubscribeToSelectedProductGroupChangedEvent();
			}
		}

		public ProductGroupPickerButtonViewController(IntPtr handle) : base(handle)
		{
			isSubscribedToSelectedProductGroupChangedEvent = false;
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			UpdateButtonLabel(ProductGroupsDataSource.SelectedProductGroup);
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);

			if (segue.Identifier.Equals(SegueIdProductGroupPickerList))
			{
				var productGroupPicker = segue.DestinationViewController as ProductGroupPickerListViewController;
				if (productGroupPicker != null)
				{
					productGroupPicker.ProductGroupsDataSource = this.ProductGroupsDataSource;
				}
			}
		}

		private void UpdateButtonLabel(ProductGroup productGroup)
		{
			if (productGroup != null && ProductGroupPickerButton != null)
			{
                ProductGroupPickerButton.SetTitle(productGroup.Name, UIControlState.Normal);
                ProductGroupPickerButton.SetTitleColor(AppDelegate.ColorTextProductGroupPicker, UIControlState.Normal);
			}
		}

		private void SelectedProductGroupChanged(ProductGroup newSelectedProductGroup)
		{
			InvokeOnMainThread(() =>
			{
				UpdateButtonLabel(newSelectedProductGroup);
			});
		}

		private void SubscribeToSelectedProductGroupChangedEvent()
		{
			if (ProductGroupsDataSource != null && !isSubscribedToSelectedProductGroupChangedEvent)
			{
				ProductGroupsDataSource.SelectedProductGroupChanged += SelectedProductGroupChanged;
				isSubscribedToSelectedProductGroupChangedEvent = true;
			}
		}

		private void UnsubscribeFromSelectedProductGroupChangedEvent()
		{
			if (ProductGroupsDataSource != null && isSubscribedToSelectedProductGroupChangedEvent)
			{
				ProductGroupsDataSource.SelectedProductGroupChanged -= SelectedProductGroupChanged;
				isSubscribedToSelectedProductGroupChangedEvent = false;
			}
		}
	}
}
