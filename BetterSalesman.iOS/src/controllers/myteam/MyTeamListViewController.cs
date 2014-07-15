using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace BetterSalesman.iOS
{
	public partial class MyTeamListViewController : UITableViewController
	{
        const string menu_icon = "ic_menu";
        
		public MyTeamListViewController (IntPtr handle) : base (handle)
		{
		}
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            // TODO my team ListViewSource
            TableView.Source = new ArgumentsListViewSource();
            
            var menuButton = new UIBarButtonItem(UIImage.FromBundle(menu_icon), UIBarButtonItemStyle.Plain, delegate
            {
                RootViewController.Navigation.ToggleMenu();
            });

            NavigationItem.SetLeftBarButtonItem(menuButton, true);
        }
	}
}
