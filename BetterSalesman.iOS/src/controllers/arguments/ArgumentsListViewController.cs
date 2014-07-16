using MonoTouch.UIKit;
using System;
using System.Diagnostics;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.iOS
{
    public partial class ArgumentsListViewController : UITableViewController
    {
        const string menu_icon = "ic_menu";
        
        public ArgumentsListViewController() : base(UITableViewStyle.Grouped)
        {
        }
        
        public ArgumentsListViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            TableView.Source = new ArgumentsListViewSource();
            
            var menuButton = new UIBarButtonItem(UIImage.FromBundle(menu_icon), UIBarButtonItemStyle.Plain, delegate
            {
                RootViewController.Navigation.ToggleMenu();
            });

            NavigationItem.SetLeftBarButtonItem(menuButton, true);
            
            UserSessionManager.Instance.FetchUser(user => Debug.WriteLine("Logged in user: " + user));
        }
    }
}

