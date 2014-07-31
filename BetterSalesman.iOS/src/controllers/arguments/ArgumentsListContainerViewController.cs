using System;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;
using BetterSalesman.Core.BusinessLayer;
using System.Diagnostics;
using System.Threading.Tasks;
using MonoTouch.Foundation;

namespace BetterSalesman.iOS
{
	partial class ArgumentsListContainerViewController : UIViewController
	{
		private const string SegueIdAddArgumentButton = "segueIdAddArgumentButton";
		private const string SegueIdProductGroupPickerEmbeded = "segueIdProductGroupPickerSubview";
		private const string SegueIdProductGroupsListEmbeded = "segueIdArgumentsListSubview";

    	private const string MenuIconName = "ic_menu";

		public ProductGroupsDataSource ProductGroupsDataSource { get; private set; }

		public ArgumentsListContainerViewController(IntPtr handle) : base(handle)
		{
			ProductGroupsDataSource = new ProductGroupsDataSource();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

      var menuButton = new UIBarButtonItem(UIImage.FromBundle(MenuIconName), UIBarButtonItemStyle.Plain, delegate
          {
              FlyoutViewController.Navigation.ToggleMenu();
          });

      NavigationItem.SetLeftBarButtonItem(menuButton, false);

			Task.Run(() =>
			{
				ProductGroupsDataSource.Initialize();
			});
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);

			if (segue.Identifier.Equals(SegueIdProductGroupPickerEmbeded))
			{
				var destinationController = segue.DestinationViewController as ProductGroupPickerButtonViewController;
				if (destinationController != null)
				{
					destinationController.ProductGroupsDataSource = ProductGroupsDataSource;
				}
			}

			LogSegue(segue);
		}

		private void LogSegue(UIStoryboardSegue segue)
		{
			Debug.WriteLine(string.Format("Segue {0} from {1} to {2}", segue.Identifier, segue.SourceViewController.GetType().Name, segue.DestinationViewController.GetType().Name));
		}
	}
}
