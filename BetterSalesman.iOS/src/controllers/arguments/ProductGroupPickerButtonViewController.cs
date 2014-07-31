using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;
using System.Diagnostics;
using BetterSalesman.Core.BusinessLayer;

namespace BetterSalesman.iOS
{
	partial class ProductGroupPickerButtonViewController : UIViewController
	{
		private const string SegueIdProductGroupPickerEmbeded = "segueIdProductGroupPickerSubview";
		private const string SegueIdProductGroupPickerList = "segueIdProductGroupPickerList";

		private ArgumentsListContainerViewController argumentsListContainerViewController;

		private bool isSubscribedToSelectedProductGroupChangedEvent;

		private ProductGroupsDataSource productsGroupDataSource;

		public ProductGroupsDataSource ProductGroupsDataSource 
		{ 
			get
			{
				return productsGroupDataSource;
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

			SubscribeToSelectedProductGroupChangedEvent();
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);

			UnsubscribeFromSelectedProductGroupChangedEvent();
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);

			LogSegue(segue);

			if (segue.Identifier.Equals(SegueIdProductGroupPickerEmbeded))
			{
				if (segue.DestinationViewController == this)
				{
					argumentsListContainerViewController = segue.SourceViewController as ArgumentsListContainerViewController;
					Debug.Assert(argumentsListContainerViewController != null, "argumentsListContainerViewController can't be null");
				}
			}
			else if (segue.Identifier.Equals(SegueIdProductGroupPickerList))
			{
				if (segue.DestinationViewController == this)
				{
					// TODO - assign picked ProductGroup to argumentsListContainerViewController
				}
			}
		}
			
		private void SelectedProductGroupChanged(ProductGroup newSelectedProductGroup)
		{
			InvokeOnMainThread(() =>
			{
				if (newSelectedProductGroup != null)
				{
					Debug.WriteLine("Selected ProductGroup changed to: " + newSelectedProductGroup.Name);
				}
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

		private void LogSegue(UIStoryboardSegue segue)
		{
			Debug.WriteLine(string.Format("Segue {0} from {1} to {2}", segue.Identifier, segue.SourceViewController.GetType().Name, segue.DestinationViewController.GetType().Name));
		}
	}
}
