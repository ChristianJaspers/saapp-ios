using System;
using MonoTouch.UIKit;

namespace BetterSalesman.iOS
{
	partial class ArgumentsListContainerViewController : UIViewController
	{
        const string menu_icon = "ic_menu";
        
		public ArgumentsListContainerViewController (IntPtr handle) : base (handle)
		{
		}
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            var menuButton = new UIBarButtonItem(UIImage.FromBundle(menu_icon), UIBarButtonItemStyle.Plain, delegate
                {
                    FlyoutViewController.Navigation.ToggleMenu();
                });

            NavigationItem.SetLeftBarButtonItem(menuButton, false);
        }
	}
}
