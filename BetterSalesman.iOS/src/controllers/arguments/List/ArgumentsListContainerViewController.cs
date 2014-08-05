using System;
using MonoTouch.UIKit;
using System.Diagnostics;
using MonoTouch.Foundation;

namespace BetterSalesman.iOS
{
    partial class ArgumentsListContainerViewController : BaseProductCategoryPickerUIViewController
	{
		private const string SegueIdArgumentsListEmbeded = "segueIdArgumentsListSubview";

    	private const string MenuIconName = "ic_menu";

		public ArgumentsListContainerViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

      		var menuButton = new UIBarButtonItem(UIImage.FromBundle(MenuIconName), UIBarButtonItemStyle.Plain, delegate
          	{
            	FlyoutViewController.Navigation.ToggleMenu();
          	});

      		NavigationItem.SetLeftBarButtonItem(menuButton, false);
		}

		protected override void OnSynchronizationFinished()
		{
			base.OnSynchronizationFinished();

			ProductGroupsDataSource.ReloadProductGroups();
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
            
			if (segue.Identifier.Equals(SegueIdArgumentsListEmbeded))
			{
				var destinationController = segue.DestinationViewController as ArgumentsListViewController;
				if (destinationController != null)
				{
					destinationController.ProductGroupsDataSource = ProductGroupsDataSource;
				}
			}
		}
	}
}
