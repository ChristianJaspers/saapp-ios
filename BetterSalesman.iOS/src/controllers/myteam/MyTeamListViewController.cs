using System;

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
            
            TableView.Source = new MyTeamListViewSource();
            
            Title = I18n.MyTeam;
            
            var menuButton = new UIBarButtonItem(UIImage.FromBundle(menu_icon), UIBarButtonItemStyle.Plain, delegate
            {
                FlyoutViewController.Navigation.ToggleMenu();
            });

            NavigationItem.SetLeftBarButtonItem(menuButton, true);
        }
	}
}
